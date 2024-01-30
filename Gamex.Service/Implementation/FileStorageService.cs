﻿using Microsoft.AspNetCore.Components.Forms;

namespace Gamex.Service.Implementation;

public class FileStorageService(Cloudinary cloudinary) : IFileStorageService
{
    private readonly Cloudinary _cloudinary = cloudinary;

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

    public async Task<FileStorageDTO> SaveFile(IBrowserFile file, string tag)
    {
        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(file.Name, file.OpenReadStream()),
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
