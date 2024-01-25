using Microsoft.AspNetCore.Components.Forms;

namespace Gamex.Service.Contract
{
    public interface IFileStorageService
    {
        Task<bool> DeleteFile(string publicId);
        Task<FileStorageDTO> SaveFile(IFormFile file, string tag);
        Task<FileStorageDTO> SaveFileForBlazor(IBrowserFile file, string tag);
    }
}