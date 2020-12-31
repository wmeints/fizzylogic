namespace FizzyLogic.Models
{
    /// <summary>
    /// Each article is filed in a category. 
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Gets or sets the ID of the category.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the slug for the article. This is used in the URL
        /// to identify a category.
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// Gets or sets the title of the category.
        /// </summary>
        public string Title { get; set; }
    }
}