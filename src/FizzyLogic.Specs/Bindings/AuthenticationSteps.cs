namespace FizzyLogic.Specs.Bindings
{
    using FizzyLogic.Specs.Models;
    using FizzyLogic.Specs.Support;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TechTalk.SpecFlow;

    [Binding]
    public class AuthenticationSteps
    {
        [Given("I am authenticated as an administrator")]
        public void AuthenticateUserStep()
        {
            _ = ApplicationEnvironment
                .Navigate<LoginPage>("/Account/Login")
                .WithUserName(ApplicationEnvironment.AdminUserName)
                .WithPassword(ApplicationEnvironment.AdminPassword)
                .Login();
        }
    }
}
