using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FizzyLogic.Data;
using FizzyLogic.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace FizzyLogic.Services
{
    public class ContentImport
    {
        public static async Task StartAsync(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                if (File.Exists("backup/database.json"))
                {
                    Console.WriteLine("Detected import file. Starting content import...");

                    dynamic backupData = await LoadBackupContentAsync();
                    ApplicationUser importUser = await LoadImportUser(userManager, configuration);
                    Dictionary<string, Category> importedCategories = LoadCategories(backupData);
                    Dictionary<string, string> importedPostCategories = LoadPostCategories(backupData);

                    List<Article> importedPosts = LoadArticles(
                        backupData,
                        importUser,
                        importedCategories,
                        importedPostCategories);

                    Console.WriteLine("Importing articles and categories into the database...");

                    await applicationDbContext.Categories.AddRangeAsync(
                        importedPosts.Select(x => x.Category).Distinct().ToList());

                    await applicationDbContext.Articles.AddRangeAsync(importedPosts);

                    await applicationDbContext.SaveChangesAsync();

                    // Make sure to move the file so we don't import it twice.
                    File.Move("backup/database.json", "backup/database.json.imported");

                    Console.WriteLine("Done importing content.");
                }
            }
        }

        private static Dictionary<string, string> LoadPostCategories(dynamic backupData)
        {
            var importedPostTags = new List<(string postId, string categoryId)>();
            dynamic postTags = backupData.data.posts_tags;

            foreach (var record in postTags)
            {
                string postId = record.post_id;
                string tagId = record.tag_id;
                int sortOrder = record.sort_order;

                if (sortOrder == 0)
                {
                    importedPostTags.Add((postId, tagId));
                }
            }

            var groupedTags = importedPostTags
                .GroupBy(x => x.postId);

            var results = groupedTags.ToDictionary(
                x => x.Key,
                x => x.First().categoryId);

            return results;
        }

        private static List<Article> LoadArticles(dynamic backupData, ApplicationUser importUser,
            Dictionary<string, Category> categories, Dictionary<string, string> articleCategories)
        {
            Console.WriteLine("Parsing article data...");

            List<Article> importedPosts = new List<Article>();
            dynamic posts = backupData.data.posts;

            foreach (dynamic post in posts)
            {
                string id = post.id;
                string title = post.title;
                string html = post.html;
                string mobileDoc = post.mobiledoc;
                string slug = post.slug;
                DateTime dateCreated = post.created_at;
                DateTime? datePublished = post.published_at;
                string status = post.status;
                string featuredImage = post.feature_image;
                string excerpt = post.custom_excerpt;

                if (excerpt == null)
                {
                    excerpt = post.plaintext.ToString().Substring(0, 200);
                }

                if (!articleCategories.ContainsKey(id))
                {
                    Console.WriteLine($"Skipped post {title} due to an invalid category configuration.");
                    continue;
                }

                var importedArticle = new Article
                {
                    Author = importUser,
                    Excerpt = excerpt,
                    Html = html,
                    Slug = slug,
                    Title = title,
                    DateCreated = dateCreated,
                    DatePublished = status switch
                    {
                        "published" => datePublished,
                        _ => null
                    },
                    DateModified = dateCreated,
                    FeaturedImage = featuredImage,
                    MobileDoc = mobileDoc,
                    Category = categories.ContainsKey(articleCategories[id]) ? categories[articleCategories[id]] : null
                };

                importedPosts.Add(importedArticle);
            }

            return importedPosts;
        }

        private static async Task<ApplicationUser> LoadImportUser(UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            Console.WriteLine("Locating import user...");

            var importUser = await userManager.FindByEmailAsync(configuration["AdminUser:EmailAddress"]);

            if (importUser == null)
            {
                Console.WriteLine("Import user not found. Creating import user...");

                importUser = new ApplicationUser()
                {
                    UserName = configuration["AdminUser:EmailAddress"],
                    Email = configuration["AdminUser:EmailAddress"],
                };

                var result = await userManager.CreateAsync(importUser, configuration["AdminUser:Password"]);

                if (!result.Succeeded)
                {
                    throw new Exception(
                        "Failed to create application user. " +
                        "Please verify the e-mail address and password settings.");
                }
            }

            return importUser;
        }

        private static Dictionary<string, Category> LoadCategories(dynamic backupData)
        {
            Console.WriteLine("Parsing category data...");

            var results = new Dictionary<string, Category>();
            dynamic tags = backupData.data.tags;

            foreach (dynamic record in tags)
            {
                string id = record.id;
                string name = record.name;
                string slug = record.slug;

                results.Add(id, new Category
                {
                    Title = name,
                    Slug = slug
                });
            }

            return results;
        }

        private static async Task<dynamic> LoadBackupContentAsync()
        {
            Console.WriteLine("Load backup content from backup/database.json");

            var fileContent = await File.ReadAllTextAsync("backup/database.json");
            return JsonConvert.DeserializeObject<dynamic>(fileContent);
        }
    }
}