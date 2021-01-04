namespace FizzyLogic.Specs.Bindings
{
    using FizzyLogic.Data;
    using FizzyLogic.Models;
    using FizzyLogic.Services;
    using FizzyLogic.Specs.Models;
    using FizzyLogic.Specs.Support;
    using FluentAssertions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using TechTalk.SpecFlow;

    [Binding]
    public class ContentManagementSteps
    {
        [Given("I am on the management dashboard")]
        public void GivenOnTheDashboardPage()
        {
            _ = ApplicationEnvironment.Navigate<AdminDashboardPage>("/Admin");
        }

        [Given(@"I have a draft article with the title ""(.*)""")]
        public async Task GivenIHaveADraftArticleWithTheTitle(string title)
        {
            _ = await ApplicationEnvironment.ContentManager.CreateDraftArticle(title);
        }

        [When(@"I create a new article with the title ""(.*)""")]
        public async Task WhenICreateANewArticleWithTheTitle(string title)
        {
            var category = await ApplicationEnvironment.ContentManager.FindFirstCategory();

            _ = ApplicationEnvironment
                .Navigate<ArticleDetailsPage>("/Admin/Articles/New")
                .WithTitle(title)
                .WithBody("test")
                .WithCategory(category.Id.ToString())
                .WithExcerpt("test");
        }

        [When(@"I save the article")]
        public void WhenISaveTheArticle()
        {
            _ = ApplicationEnvironment.AsPage<ArticleDetailsPage>().Save();
        }

        [Then(@"a (draft|published) article exists with the title ""(.*)""")]
        public async Task ThenANewDraftArticleIsCreatedWithTheTitle(string status, string title)
        {
            var article = await ApplicationEnvironment.ContentManager.FindArticleByTitle(title);

            _ = article.Should().NotBeNull();

            if (status == "draft")
            {
                _ = article.DatePublished.Should().BeNull("Expected status draft");
            }
            else if (status == "published")
            {
                _ = article.DatePublished.Should().NotBeNull("Expected status published");
            }
        }

        [When(@"I am editing the article with the title ""(.*)""")]
        public async Task WhenIAmEditingTheArticleWithTheTitle(string title)
        {
            var article = await ApplicationEnvironment.ContentManager.FindArticleByTitle(title);
            _ = ApplicationEnvironment.Navigate<ArticleDetailsPage>($"/Admin/Articles/Edit/{article.Id}");
        }

        [When(@"I publish the article")]
        public void WhenIPublishTheArticle()
        {
            _ = ApplicationEnvironment.AsPage<ArticleDetailsPage>().Publish();
        }

    }
}
