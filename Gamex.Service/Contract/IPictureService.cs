namespace Gamex.Service.Contract
{
    public interface IPictureService
    {
        Task<PictureDTO> CreatePicture(PictureCreateDTO pictureCreateDTO);
        Task<bool> DeletePicture(Guid pictureId);
        Task<PictureDTO> GetPicture(Guid pictureId);
        Task<PictureDTO> UpdatePicture(PictureUpdateDTO pictureUpdateDTO);
    }
}