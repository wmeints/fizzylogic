namespace FizzyLogic.Specs.Support
{
    using FizzyLogic.Data;
    using FizzyLogic.Models;
    using FizzyLogic.Specs.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;

    /// <summary>
    /// Provides services for the application under test.
    /// </summary>
    public static class ApplicationEnvironment
    {
        private static Process _applicationProcess;
        private static int _attemptsRemaining;
        private static IServiceCollection _serviceCollection;
        private static IConfiguration _configuration;
        private static ChromeDriver _driver;

        /// <summary>
        /// The username to use for logging in with the test user.
        /// </summary>
        public const string AdminUserName = "test@domain.org";

        /// <summary>
        /// The password to use for logging in with the test user.
        /// </summary>
        public const string AdminPassword = "TestUser123!";

        /// <summary>
        /// The base URL For the application.
        /// </summary>
        public const string BaseUrl = "https://localhost:5001/";

        /// <summary>
        /// Shuts down the server process.
        /// </summary>
        public static void Shutdown()
        {
            if (!_applicationProcess.HasExited)
            {
                _applicationProcess.Kill();
            }
        }

        /// <summary>
        /// Initializes the application environment.
        /// </summary>
        public static void Initialize()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Test.json", optional: true)
                .AddUserSecrets(typeof(Startup).Assembly)
                .AddEnvironmentVariables()
                .Build();

            _serviceCollection = new ServiceCollection()
                .AddLogging(logging => ConfigureLogging(_configuration, logging));

            var startup = new Startup(_configuration);
            startup.ConfigureServices(_serviceCollection);

            ServiceProvider = _serviceCollection.BuildServiceProvider();

            Client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:5001")
            };

            ContentManager = new ContentManager(ServiceProvider);

            Reset();
        }

        private static void ConfigureLogging(IConfiguration configuration, ILoggingBuilder logging)
        {
            _ = logging
                .AddConfiguration(configuration.GetSection("Logging"))
                .AddConsole()
                .AddDebug();
        }

        /// <summary>
        /// Resets the application environment.
        /// </summary>
        public static void Reset()
        {
            using var scope = ServiceProvider.CreateScope();

            var administrator = new ApplicationUser
            {
                Email = AdminUserName,
                UserName = AdminUserName,
                EmailConfirmed = true
            };

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Drop and recreate the database. This ensures we've got everything ready to go.
            _ = applicationDbContext.Database.EnsureDeleted();
            _ = applicationDbContext.Database.EnsureCreated();

            // Create a new application user.
            userManager.CreateAsync(administrator, AdminPassword).Wait();

            // Create a reference category.
            _ = applicationDbContext.Categories.Add(new Category() { Title = "Other", Slug = "other" });
            var rowsAffected = applicationDbContext.SaveChanges();

        }

        /// <summary>
        /// Starts the application process.
        /// </summary>
        public static void Start()
        {
            var startInfo = new ProcessStartInfo("dotnet", "run")
            {
                WorkingDirectory = Path.GetFullPath("../../../../FizzyLogic")
            };

            _applicationProcess = Process.Start(startInfo);
            _attemptsRemaining = 10;

            while (_attemptsRemaining > 0)
            {
                var response = Client.GetAsync("/").Result;

                if (response.IsSuccessStatusCode)
                {
                    return;
                }
                else
                {
                    _attemptsRemaining--;

                    if (_attemptsRemaining == 0)
                    {
                        var responseText = response.Content.ReadAsStringAsync().Result;
                        throw new Exception($"Failed to start the website after 10 attempts: {responseText}");
                    }
                }

                Thread.Sleep(500);
            }
        }

        /// <summary>
        /// Initializes the browser to talk to the website.
        /// </summary>
        public static void InitializeBrowser()
        {
            var chromeOptions = new ChromeOptions();

            chromeOptions.AddArgument("headless");
            chromeOptions.AddArgument("disable-gpu");

            _driver = new ChromeDriver(chromeOptions);
        }

        /// <summary>
        /// Closes the browser and shuts it down.
        /// </summary>
        public static void CloseBrowser()
        {
            Browser.Close();
            Browser.Quit();
        }

        /// <summary>
        /// Navigates to an application page.
        /// </summary>
        /// <typeparam name="TPage">Type of page to return when the navigation is completed.</typeparam>
        /// <param name="relativeUrl">The relative URL within the application including any optional query string parameters.</param>
        public static TPage Navigate<TPage>(string relativeUrl) where TPage : PageObject
        {
            var absoluteUri = new Uri(new Uri(BaseUrl), relativeUrl);
            Browser.Navigate().GoToUrl(absoluteUri.ToString());

            // Wait for the page to render. 
            // This can take a few miliseconds because of Razor behavior.
            Thread.Sleep(100);

            return AsPage<TPage>();
        }


        /// <summary>
        /// Creates a new page object representing the current page we're at in the application environment.
        /// </summary>
        /// <typeparam name="TPage">Type of page to return.</typeparam>
        /// <returns>Returns the page object representing the current page in the application.</returns>
        public static TPage AsPage<TPage>() where TPage : PageObject
        {
            return (TPage)Activator.CreateInstance(typeof(TPage), new object[] { Browser, JavaScript });
        }

        /// <summary>
        /// Gets the service provider for any services that we may need during a test run.
        /// </summary>
        public static IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// Gets the HTTP client to execute HTTP requests against the application.
        /// </summary>
        public static HttpClient Client { get; private set; }

        /// <summary>
        /// Gets the browser to navigate the application.
        /// </summary>
        public static IWebDriver Browser => _driver;

        /// <summary>
        /// Gets the javascript executor for the application.
        /// </summary>
        public static IJavaScriptExecutor JavaScript => _driver;

        /// <summary>
        /// Gets the content manager for the application.
        /// </summary>
        public static ContentManager ContentManager { get; private set; }
    }
}
