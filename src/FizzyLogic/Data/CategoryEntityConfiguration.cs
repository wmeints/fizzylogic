namespace FizzyLogic.Data
{
    using FizzyLogic.Models;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Provides model configuration for the <see cref="FizzyLogic.Models.Category"/> entity.
    /// </summary>
    public class CategoryEntityConfiguration : IEntityTypeConfiguration<Category>
    {
        /// <summary>
        /// Provides the model configuration for the entity.
        /// </summary>
        /// <param name="builder">Entity model builder instance.</param>
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Category> builder)
        {
            _ = builder.Property(x => x.Title).IsRequired().HasMaxLength(500);
            _ = builder.Property(x => x.Slug).IsRequired().HasMaxLength(500);
        }
    }
}