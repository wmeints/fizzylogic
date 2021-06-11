namespace FizzyLogic.Services
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.Processing;

    /// <summary>
    /// Implementation of the image service.
    /// </summary>
    public class ImageService : IImageService
    {
        private readonly IClock _clock;

        /// <summary>
        /// Initializes a new instance of <see cref="ImageService"/>
        /// </summary>
        /// <param name="clock">Clock implementation to use.</param>
        public ImageService(IClock clock)
        {
            _clock = clock;
        }

        /// <summary>
        /// Uploads an image to the content folder.
        /// </summary>
        /// <param name="filename">Filename for the image.</param>
        /// <param name="imageStream">Stream containing the image data.</param>
        /// <returns>Returns the content path on the website for the image.</returns>
        public async Task<string> UploadImage(string filename, Stream imageStream)
        {
            var today = _clock.UtcNow;

            var outputFolder = Path.Join(
                "wwwroot/content/images/",
                today.Year.ToString("0000"),
                today.Month.ToString("00"),
                today.Day.ToString("00"));

            await SaveImage(filename, imageStream, outputFolder);
            await CreateResponsiveImages(filename, outputFolder);

            return $"/content/images/{today.Year:0000}/{today.Month:00}/{today.Day:00}/{filename}";
        }

        private static async Task SaveImage(string filename, Stream imageStream, string outputFolder)
        {
            if (!Directory.Exists(outputFolder))
            {
                _ = Directory.CreateDirectory(outputFolder);
            }

            var outputFileName = Path.Join(outputFolder, filename);
            var bytesRead = 0;
            var buffer = new byte[4096];

            using (var outputStream = File.OpenWrite(outputFileName))
            {
                while ((bytesRead = await imageStream.ReadAsync(buffer, 0, 4096)) > 0)
                {
                    await outputStream.WriteAsync(buffer, 0, bytesRead);
                }
            }
        }

        private async Task CreateResponsiveImages(string filename, string outputFolder)
        {
            var outputFileName = Path.Join(outputFolder, filename);
            var targetSizes = new[] { 966, 514 };

            foreach (var targetSize in targetSizes)
            {
                await ResizeImage(outputFileName, Path.Join(outputFolder,
                    GenerateFileName(filename, targetSize)), targetSize);
            }
        }

        private async Task ResizeImage(string originalFile, string outputFile, int targetWidth)
        {
            var image = Image.Load(originalFile);
            var targetHeight = (targetWidth / image.Width) * image.Height;

            image.Mutate(x => x.Resize(targetWidth, targetHeight));
            await image.SaveAsync(outputFile);
        }

        private string GenerateFileName(string filename, int targetWidth)
        {
            var nameWithoutExtension = Path.GetFileNameWithoutExtension(filename);
            var extension = Path.GetExtension(filename);

            return $"{nameWithoutExtension}-w{targetWidth}{extension}";
        }
    }
}