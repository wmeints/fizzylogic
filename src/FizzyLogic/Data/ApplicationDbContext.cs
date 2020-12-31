namespace FizzyLogic.Data
{
    using FizzyLogic.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Provides a central endpoint for interacting with the database.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ApplicationDbContext"/>.
        /// </summary>
        /// <param name="options">Options for the DbContext instance.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        /// <summary>
        /// Gets or sets the DbSet for articles.
        /// </summary>
        public DbSet<Article> Articles { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for categories.
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Configures the model that the DbContext provides.
        /// </summary>
        /// <param name="modelBuilder">Model builder instance for configuring the model.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            _ = modelBuilder.ApplyConfiguration(new ArticleEntityConfiguration());
            _ = modelBuilder.ApplyConfiguration(new CategoryEntityConfiguration());
        }
    }
}