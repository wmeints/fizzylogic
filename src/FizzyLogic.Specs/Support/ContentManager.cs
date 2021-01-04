namespace FizzyLogic.Specs.Support
{
    using FizzyLogic.Data;
    using FizzyLogic.Models;
    using FizzyLogic.Services;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ContentManager
    {
        private readonly IServiceProvider _serviceProvider;

        public ContentManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<Category> FindFirstCategory()
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
            var dbContext = dbContextFactory.CreateDbContext();

            return await dbContext.Categories.OrderBy(x => x.Title).FirstOrDefaultAsync();
        }

        public async Task<Article> FindArticleById(int id)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
            var dbContext = dbContextFactory.CreateDbContext();

            return await dbContext.Articles.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Article> FindArticleByTitle(string title)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
            var dbContext = dbContextFactory.CreateDbContext();

            return await dbContext.Articles.FirstOrDefaultAsync(x => x.Title == title);
        }

        public async Task<Article> CreateDraftArticle(string title)
        {
            using var scope = ApplicationEnvironment.ServiceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
            var applicationDbContext = dbContextFactory.CreateDbContext();

            var author = await userManager.FindByEmailAsync(ApplicationEnvironment.AdminUserName);
            var category = await applicationDbContext.Categories.FirstOrDefaultAsync();

            var draftArticle = new Article
            {
                Title = title,
                Slug = new Slugifier().Process(title),
                Markdown = "test",
                Author = author,
                Category = category,
                Html = "<h1>Test</h1>",
                DateCreated = DateTime.UtcNow
            };

            _ = applicationDbContext.Attach(author);

            _ = await applicationDbContext.Articles.AddAsync(draftArticle);
            _ = await applicationDbContext.SaveChangesAsync();

            return draftArticle;
        }
    }
}
