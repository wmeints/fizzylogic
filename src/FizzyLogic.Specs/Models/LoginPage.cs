namespace FizzyLogic.Specs.Models
{
    using OpenQA.Selenium;

    /// <summary>
    /// Models the login page for the admin panel.
    /// </summary>
    public class LoginPage : PageObject
    {
        /// <summary>
        /// Initializes a new instance of <see cref="LoginPage"/>.
        /// </summary>
        /// <param name="driver"></param>
        public LoginPage(IWebDriver driver, IJavaScriptExecutor javaScript) : base(driver, javaScript)
        {

        }

        /// <summary>
        /// Fills in the username for the user.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public LoginPage WithUserName(string value)
        {
            var inputElement = Driver.FindElement(By.Name("Input.EmailAddress"));
            inputElement.SendKeys(value);
            return this;
        }

        /// <summary>
        /// Fills in the password for the user
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public LoginPage WithPassword(string value)
        {
            var inputElement = Driver.FindElement(By.Name("Input.Password"));
            inputElement.SendKeys(value);

            return this;
        }

        /// <summary>
        /// Clicks the login button
        /// </summary>
        /// <returns></returns>
        public AdminDashboardPage Login()
        {
            var submitButton = Driver.FindElement(By.CssSelector("button[type='submit']"));
            submitButton.Click();

            return new AdminDashboardPage(Driver, JavaScript);
        }
    }
}
