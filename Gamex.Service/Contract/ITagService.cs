namespace Gamex.Service.Contract
{
    public interface ITagService
    {
        /// <summary>
        /// Creates a new tag.
        /// </summary>
        /// <param name="tagCreateDTO">The DTO containing the information for creating the tag.</param>
        /// <returns>The created tag, or null if creation fails.</returns>
        Task<TagDTO?> CreateTag(TagCreateDTO tagCreateDTO);

        /// <summary>
        /// Deletes a tag by its ID.
        /// </summary>
        /// <param name="id">The ID of the tag to delete.</param>
        /// <returns>True if the tag is successfully deleted, false otherwise.</returns>
        Task<bool> DeleteTag(Guid id);

        /// <summary>
        /// Retrieves all tags.
        /// </summary>
        /// <returns>An IQueryable of TagDTO representing all tags.</returns>
        IQueryable<TagDTO> GetAllTags();

        /// <summary>
        /// Retrieves a tag by its ID.
        /// </summary>
        /// <param name="id">The ID of the tag to retrieve.</param>
        /// <returns>The tag with the specified ID, or null if not found.</returns>
        Task<TagDTO?> GetTagById(Guid id);

        /// <summary>
        /// Retrieves a tag by its name.
        /// </summary>
        /// <param name="name">The name of the tag to retrieve.</param>
        /// <returns>The tag with the specified name.</returns>
        Task<TagDTO> GetTagByName(string name);

        /// <summary>
        /// Updates a tag.
        /// </summary>
        /// <param name="tagUpdateDTO">The DTO containing the information for updating the tag.</param>
        /// <returns>True if the tag is successfully updated, false otherwise.</returns>
        Task<bool> UpdateTag(TagUpdateDTO tagUpdateDTO);
    }
}