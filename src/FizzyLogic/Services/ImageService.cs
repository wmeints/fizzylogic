namespace FizzyLogic.Services
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public interface IImageService
    {
        Task<string> UploadImage(string filename, Stream imageStream);
    }

    public class ImageService : IImageService
    {
        public async Task<string> UploadImage(string filename, Stream imageStream)
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

            var bytesRead = 0;
            var buffer = new byte[4096];

            using (var outputStream = File.OpenWrite(Path.Join(outputFolder, filename)))
            {
                while ((bytesRead = await imageStream.ReadAsync(buffer, 0, 4096)) > 0)
                {
                    await outputStream.WriteAsync(buffer, 0, bytesRead);
                }
            }

            return $"/content/{today.Year:0000}/{today.Month:00}/{today.Day:00}/{filename}";
        }
    }
}