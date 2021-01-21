namespace FizzyLogic.Specs.Bindings
{
    using FizzyLogic.Specs.Models;
    using FizzyLogic.Specs.Support;
    using TechTalk.SpecFlow;

    [Binding]
    public class AuthenticationSteps
    {
        [Given("I am am logged in as an administrator")]
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
