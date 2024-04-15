using Microsoft.AspNetCore.Components.Forms;

namespace Gamex.Service.Contract
{
    public interface IFileStorageService
    {
        /// <summary>
        /// Deletes a file with the specified public ID.
        /// </summary>
        /// <param name="publicId">The public ID of the file to delete.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a boolean value indicating whether the file was successfully deleted.</returns>
        Task<bool> DeleteFile(string publicId);

        /// <summary>
        /// Saves a file with the specified tag using the provided IFormFile.
        /// </summary>
        /// <param name="file">The IFormFile representing the file to save.</param>
        /// <param name="tag">The tag to associate with the file.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a FileStorageDTO object representing the saved file.</returns>
        Task<FileStorageDTO> SaveFile(IFormFile file, string tag);

        /// <summary>
        /// Saves a file with the specified tag using the provided IBrowserFile.
        /// </summary>
        /// <param name="file">The IBrowserFile representing the file to save.</param>
        /// <param name="tag">The tag to associate with the file.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a FileStorageDTO object representing the saved file.</returns>
        Task<FileStorageDTO> SaveFile(IBrowserFile file, string tag);
    }
}