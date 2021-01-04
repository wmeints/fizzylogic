namespace FizzyLogic.Specs.Models
{
    using OpenQA.Selenium;

    /// <summary>
    /// Classes that derive from this class implement page objects.
    /// </summary>
    public abstract class PageObject
    {
        /// <summary>
        /// The number of miliseconds to wait for an action to complete in Blazor.
        /// </summary>
        public const int DefaultWaitTimeout = 500;

        /// <summary>
        /// Initializes a new instance of <see cref="PageObject"/>.
        /// </summary>
        /// <param name="driver">Web driver for the page.</param>
        /// <param name="javaScript">Javascript engine for the page.</param>
        protected PageObject(IWebDriver driver, IJavaScriptExecutor javaScript)
        {
            Driver = driver;
            JavaScript = javaScript;
        }

        /// <summary>
        /// Gets the driver for the page.
        /// </summary>
        protected IWebDriver Driver { get; }

        /// <summary>
        /// Gets the javascript engine for executing script logic.
        /// </summary>
        public IJavaScriptExecutor JavaScript { get; }
    }
}
