namespace FizzyLogic.Data
{
    using FizzyLogic.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ArticleEntityConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            _ = builder.Property(x => x.Title).IsRequired().HasMaxLength(500);
            _ = builder.Property(x => x.Slug).IsRequired().HasMaxLength(500);
            _ = builder.Property(x => x.Html).IsRequired();
            _ = builder.Property(x => x.DateCreated).IsRequired();

            _ = builder.HasOne(x => x.Author);
        }
    }
}