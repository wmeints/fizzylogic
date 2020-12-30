using System;
using FizzyLogic.Data;
using FizzyLogic.Models;
using FizzyLogic.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FizzyLogic
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // This data context is used for the razor components.
            // We need to create an instance per operation.
            services.AddDbContextFactory<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(_configuration.GetConnectionString("DefaultDatabase"));
            });
            
            // This data context is used for the razor pages. 
            // ASP.NET Core will make sure we have an instance per request.
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(_configuration.GetConnectionString("DefaultDatabase"));
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication();
            services.AddAuthorization();

            services.AddControllers();
            
            services.AddServerSideBlazor(options =>
            {
                options.DetailedErrors = true;
            });
            
            services
                .AddRazorPages()
                .AddFluentValidation(options =>
                {
                    options.RegisterValidatorsFromAssembly(typeof(Startup).Assembly);
                });

            services.AddSingleton<Slugifier>();
            services.AddSingleton<IImageService,ImageService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            ContentImport.StartAsync(serviceProvider, configuration).Wait();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // This middleware makes sure that everything operates as expected behind a reverse proxy.
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToPage("/Admin/{**segment}", "/Admin/_Host");
            });

        }
    }
}
