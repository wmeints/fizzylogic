using System;
using FizzyLogic.Data;
using FizzyLogic.Models;
using FizzyLogic.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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

            services.AddServerSideBlazor();
            
            services
                .AddRazorPages()
                .AddFluentValidation(options =>
                {
                    options.RegisterValidatorsFromAssembly(typeof(Startup).Assembly);
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            ContentImport.StartAsync(serviceProvider, configuration).Wait();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapRazorPages();
            });
        }
    }
}
