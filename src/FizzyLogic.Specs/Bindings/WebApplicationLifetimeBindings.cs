namespace FizzyLogic.Specs.Bindings
{
    using FizzyLogic.Specs.Support;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Ensures that the application is ready to go at the start of the test run.
    /// </summary>
    [Binding]
    public class WebApplicationLifetimeBindings
    {
        /// <summary>
        /// Initializes the application and opens a browser connection to it.
        /// </summary>
        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            ApplicationEnvironment.Initialize();
            ApplicationEnvironment.Start();
            ApplicationEnvironment.InitializeBrowser();
        }

        /// <summary>
        /// Shuts down the browser connection and application at the end of the test run.
        /// </summary>
        [AfterTestRun]
        public static void AfterTestRun()
        {
            ApplicationEnvironment.CloseBrowser();
            ApplicationEnvironment.Shutdown();
            ApplicationEnvironment.Reset();
        }
    }
}
