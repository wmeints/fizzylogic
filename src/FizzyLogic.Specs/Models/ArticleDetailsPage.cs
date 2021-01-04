namespace FizzyLogic.Specs.Models
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using System.Threading;

    /// <summary>
    /// Page object for /Admin/Articles/Edit/{id}
    /// </summary>
    public class ArticleDetailsPage : PageObject
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CreateArticlePage"/>
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="javaScript"></param>
        public ArticleDetailsPage(IWebDriver driver, IJavaScriptExecutor javaScript) : base(driver, javaScript)
        {

        }

        /// <summary>
        /// Specifies the title for the new article.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ArticleDetailsPage WithTitle(string value)
        {
            var textElement = Driver.FindElement(By.Id("title"));

            textElement.Clear();
            textElement.SendKeys(value);

            return this;
        }

        /// <summary>
        /// Specifies the excerpt for the new article.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ArticleDetailsPage WithExcerpt(string value)
        {
            var textElement = Driver.FindElement(By.Id("excerpt"));

            textElement.Clear();
            textElement.SendKeys(value);

            return this;
        }

        /// <summary>
        /// Specifies the category for the new article.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ArticleDetailsPage WithCategory(string value)
        {
            _ = ToggleSettings();

            var selectElement = new SelectElement(Driver.FindElement(By.Id("category")));
            selectElement.SelectByValue(value);

            return this;
        }

        /// <summary>
        /// Specifies the content for the new article.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ArticleDetailsPage WithBody(string value)
        {
            _ = JavaScript.ExecuteScript($"contentEditor.setContent(\"{value}\");");
            return this;
        }

        /// <summary>
        /// Specifies the header image for the article.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ArticleDetailsPage WithHeaderImage(string value)
        {
            var element = Driver.FindElement(By.Id("headerImage"));

            element.Clear();
            element.SendKeys(value);

            return this;
        }

        /// <summary>
        /// Saves the article.
        /// </summary>
        /// <returns></returns>
        public ArticleDetailsPage Save()
        {
            Driver.FindElement(By.Id("saveArticle")).Click();
            Thread.Sleep(DefaultWaitTimeout); // HACK: Let the page settle for a bit, so we know the action is invoked.

            return this;
        }

        /// <summary>
        /// Publishes the article.
        /// </summary>
        /// <returns></returns>
        public ArticleDetailsPage Publish()
        {
            Driver.FindElement(By.Id("publishArticle")).Click();
            Thread.Sleep(DefaultWaitTimeout); // HACK: Let the page settle for a bit, so we know the action is invoked.

            return this;
        }

        /// <summary>
        /// Toggles the setting dialog.
        /// </summary>
        /// <returns></returns>
        public ArticleDetailsPage ToggleSettings()
        {
            Driver.FindElement(By.Id("toggleSettingsPanel")).Click();
            Thread.Sleep(DefaultWaitTimeout); // HACK: Let the page settle for a bit, so we know the action is invoked.

            return this;
        }
    }
}
