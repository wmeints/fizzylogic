using FizzyLogic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FizzyLogic.Data
{
    public class ArticleEntityConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.Property(x => x.Title).IsRequired().HasMaxLength(500);
            builder.Property(x => x.Slug).IsRequired().HasMaxLength(500);
            builder.Property(x => x.Html).IsRequired();
            builder.Property(x => x.DateCreated).IsRequired();
        }
    }
}