// unset

namespace FizzyLogic.Forms
{
    public class PublishArticleForm
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Markdown { get; set; }
        public string Slug { get; set; }
        public string Excerpt { get; set; }
        public string Category { get; set; }
        public bool Draft { get; set; }
        public string FeaturedImage { get; set; }
    }
}