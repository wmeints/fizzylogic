using System;
using System.Threading.Tasks;
using FizzyLogic.Data;
using FizzyLogic.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FizzyLogic.Services
{
    public static class InitialSetup
    {
        /// <summary>
        /// Ensures that the website has a super user. 
        /// </summary>
        public static async Task EnsureSuperUser(IConfiguration configuration, IServiceProvider serviceProvider) 
        {
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("FizzyLogic");

            if(configuration.GetSection("SuperUser") == null)
            {
                logger.LogInformation("Couldn't find a 'SuperUser' " + 
                    "section in the configuration. Skipping super user setup.");

                return;
            }

            if(configuration["SuperUser:EmailAddress"] == null)
            {
                logger.LogWarning("Can't find e-mail address for the super user." +
                    " Please make sure you have your super user configured correctly.");

                return;
            }

            if(configuration["SuperUser:Password"] == null)
            {
                logger.LogWarning("Can't find password for the super user. " +
                    "Please make sure you have your super user configured correctly.");

                return;
            }

            using(var scope = serviceProvider.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var superUser = await userManager.FindByEmailAsync(configuration["SuperUser:EmailAddress"]);

                if(superUser == null)
                {
                    logger.LogInformation("Super user doesn't exist yet. Creating a new super user.");

                    superUser = new ApplicationUser
                    {
                        UserName = configuration["SuperUser:EmailAddress"],
                        Email = configuration["SuperUser:EmailAddress"],
                        EmailConfirmed = true
                    };

                    await userManager.CreateAsync(superUser, configuration["SuperUser:Password"]);
                }
            }
        }
    }
}