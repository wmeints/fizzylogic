namespace FizzyLogic.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using FizzyLogic.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Handles file uploads from the markdown editor in the admin panel.
    /// </summary>
    [ApiController]
    [Authorize(AuthenticationSchemes = "Identity.Application,ApiKey")]
    public class FileUploadsController : ControllerBase
    {
        private readonly List<string> _allowedFileTypes = new()
        {
            ".png",
            ".jpg",
            ".jpeg",
            ".gif"
        };

        private readonly IImageService _imageService;

        /// <summary>
        /// Initializes a new instance of <see cref="FileUploadsController"/>.
        /// </summary>
        /// <param name="imageService">The service to handle the image data.</param>
        public FileUploadsController(IImageService imageService)
        {
            _imageService = imageService;
        }

        /// <summary>
        /// Handles an incoming file from the markdown editor
        /// </summary>
        /// <returns>Returns 202 when the image is stored. Otherwise returns 400 for bad input.</returns>
        [HttpPost]
        [Route("/api/images")]
        public async Task<IActionResult> UploadImage()
        {
            var file = Request.Form.Files.First();
            var fileInfo = new FileInfo(file.FileName);

            if (!_allowedFileTypes.Contains(fileInfo.Extension))
            {
                ModelState.AddModelError("", "This file extension is not allowed here.");
            }

            if (ModelState.IsValid)
            {
                var today = DateTime.UtcNow;

                var outputFolder = Path.Join(
                    "wwwroot/content/images/",
                    today.Year.ToString("0000"),
                    today.Month.ToString("00"),
                    today.Day.ToString("00"));

                if (!Directory.Exists(outputFolder))
                {
                    _ = Directory.CreateDirectory(outputFolder);
                }

                using var imageStream = file.OpenReadStream();
                var contentPath = await _imageService.UploadImage(file.FileName, imageStream);

                return Ok(new { url = contentPath });
            }

            return BadRequest(ModelState);
        }
    }
}