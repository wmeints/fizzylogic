namespace FizzyLogic.Services
{
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides centralized logic to process uploaded images.
    /// </summary>
    public interface IImageService
    {
        /// <summary>
        /// Uploads an image to the content folder.
        /// </summary>
        /// <param name="filename">Filename for the image.</param>
        /// <param name="imageStream">Stream containing the image data.</param>
        /// <returns>Returns the content path on the website for the image.</returns>
        Task<string> UploadImage(string filename, Stream imageStream);
    }
}