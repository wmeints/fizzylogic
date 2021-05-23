namespace FizzyLogic.Controllers
{
    using Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// API controller for listing the categories in the website.
    /// </summary>
    [ApiController]
    [Route("/api/categories")]
    [Authorize(AuthenticationSchemes = "Identity.Application,ApiKey")]
    public class CategoriesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;

        /// <summary>
        /// Initializes a new instance of <see cref="CategoriesApiController"/>.
        /// </summary>
        /// <param name="applicationDbContext">Application DB Context.</param>
        public CategoriesApiController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        
        /// <summary>
        /// Retrieves all available categories on the website.
        /// </summary>
        /// <returns>Returns the list of categories on the website.</returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _applicationDbContext.Categories
                .OrderBy(x => x.Title).ToListAsync();
            
            return Ok(categories);
        }
    }
}