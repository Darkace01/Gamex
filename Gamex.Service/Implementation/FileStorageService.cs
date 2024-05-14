using Gamex.Common;
using Microsoft.AspNetCore.Components.Forms;

namespace Gamex.Service.Implementation;

/// <summary>
/// Service for storing files in Cloudinary.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="FileStorageService"/> class.
/// </remarks>
/// <param name="cloudinary">The Cloudinary instance.</param>
public class FileStorageService(Cloudinary cloudinary) : IFileStorageService
{
    private readonly Cloudinary _cloudinary = cloudinary;

    /// <summary>
    /// Saves a file to Cloudinary.
    /// </summary>
    /// <param name="file">The file to be saved.</param>
    /// <param name="tag">The tag for the file.</param>
    /// <returns>The file storage information.</returns>
    public async Task<FileStorageDTO> SaveFile(IFormFile file, string tag)
    {
        using var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(file.FileName, stream),
            UniqueFilename = true,
            Tags = tag
        };
        var uploadResult = await _cloudinary.UploadAsync(uploadParams).ConfigureAwait(false);

        if (uploadResult.StatusCode != HttpStatusCode.OK)
        {
            throw new Exception(
                $"Error uploading file to cloudinary. Status code: {uploadResult.StatusCode}. Error message: {uploadResult.Error.Message}");
        }
        FileStorageDTO fileStorage = new()
        {
            FileUrl = uploadResult.SecureUrl?.AbsoluteUri,
            PublicId = uploadResult.PublicId
        };
        return fileStorage;
    }

    /// <summary>
    /// Saves a file to Cloudinary.
    /// </summary>
    /// <param name="file">The file to be saved.</param>
    /// <param name="tag">The tag for the file.</param>
    /// <returns>The file storage information.</returns>
    public async Task<FileStorageDTO> SaveFile(IBrowserFile file, string tag)
    {
        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(file.Name, file.OpenReadStream(maxAllowedSize: AppConstant.MaxFileSize)),
            UniqueFilename = true,
            Tags = tag
        };
        var uploadResult = await _cloudinary.UploadAsync(uploadParams).ConfigureAwait(false);

        if (uploadResult.StatusCode != HttpStatusCode.OK)
        {
            throw new Exception(
                               $"Error uploading file to cloudinary. Status code: {uploadResult.StatusCode}. Error message: {uploadResult.Error.Message}");
        }
        FileStorageDTO fileStorage = new()
        {
            FileUrl = uploadResult.SecureUrl?.AbsoluteUri,
            PublicId = uploadResult.PublicId
        };
        return fileStorage;
    }

    /// <summary>
    /// Deletes a file from Cloudinary.
    /// </summary>
    /// <param name="publicId">The public ID of the file.</param>
    /// <returns>True if the file is deleted successfully, otherwise false.</returns>
    public async Task<bool> DeleteFile(string publicId)
    {
        var deleteResult = await _cloudinary.DeleteResourcesAsync(publicId).ConfigureAwait(false);

        if (deleteResult.StatusCode != HttpStatusCode.OK)
        {
            return false;
        }

        return true;
    }
}
