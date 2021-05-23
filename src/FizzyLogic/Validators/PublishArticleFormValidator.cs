// unset

namespace FizzyLogic.Validators
{
    using Data;
    using FluentValidation;
    using Forms;
    using System.Linq;

    public class PublishArticleFormValidator : AbstractValidator<PublishArticleForm>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public PublishArticleFormValidator(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;

            _ = RuleFor(x => x.Title).NotEmpty().MaximumLength(500);
            _ = RuleFor(x => x.Markdown).NotEmpty();

            _ = RuleFor(x => x.Category)
                .NotEmpty()
                .Must(BeExistingCategory).WithMessage("Please specify an existing category");

            _ = RuleFor(x => x.Excerpt).MaximumLength(250);

            RuleSet("Creating", () => { });

            RuleSet("Updating", () =>
            {
                _ = RuleFor(x => x.Id).Must(BeExistingArticle);
                _ = RuleFor(x => x.Draft).Must(NotUnpublishExistingArticle);
            });
        }

        private bool BeExistingArticle(int? id)
        {
            return id == null || _applicationDbContext.Articles.Any(x => x.Id == id.Value);
        }

        private bool NotUnpublishExistingArticle(PublishArticleForm form, bool draft)
        {
            var article = _applicationDbContext.Articles.FirstOrDefault(x => x.Id == form.Id.Value);
            return article?.DatePublished == null || (!draft && article.DatePublished != null);
        }

        private bool BeExistingCategory(string category)
        {
            return _applicationDbContext.Categories.Any(x => x.Slug == category);
        }
    }
}