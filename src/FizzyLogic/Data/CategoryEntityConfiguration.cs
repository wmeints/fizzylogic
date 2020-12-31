namespace FizzyLogic.Data
{
    using FizzyLogic.Models;
    using Microsoft.EntityFrameworkCore;

    public class CategoryEntityConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Category> builder)
        {
            _ = builder.Property(x => x.Title).IsRequired().HasMaxLength(500);
            _ = builder.Property(x => x.Slug).IsRequired().HasMaxLength(500);
        }
    }
}