namespace Gamex.Service.Contract
{
    public interface IPictureService
    {
        /// <summary>
        /// Creates a new picture.
        /// </summary>
        /// <param name="pictureCreateDTO">The DTO containing the picture data.</param>
        /// <returns>The created picture.</returns>
        Task<PictureDTO> CreatePicture(PictureCreateDTO pictureCreateDTO);

        /// <summary>
        /// Deletes a picture by its ID.
        /// </summary>
        /// <param name="pictureId">The ID of the picture to delete.</param>
        /// <returns>True if the picture was successfully deleted, otherwise false.</returns>
        Task<bool> DeletePicture(Guid pictureId);

        /// <summary>
        /// Retrieves a picture by its ID.
        /// </summary>
        /// <param name="pictureId">The ID of the picture to retrieve.</param>
        /// <returns>The retrieved picture.</returns>
        Task<PictureDTO> GetPicture(Guid pictureId);

        /// <summary>
        /// Retrieves a picture by its public ID.
        /// </summary>
        /// <param name="publicId">The public ID of the picture to retrieve.</param>
        /// <returns>The retrieved picture.</returns>
        Task<PictureDTO> GetPictureByPublicId(string publicId);

        /// <summary>
        /// Updates a picture.
        /// </summary>
        /// <param name="pictureUpdateDTO">The DTO containing the updated picture data.</param>
        /// <returns>The updated picture.</returns>
        Task<PictureDTO> UpdatePicture(PictureUpdateDTO pictureUpdateDTO);
    }
}