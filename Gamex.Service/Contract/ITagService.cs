namespace Gamex.Service.Contract
{
    public interface ITagService
    {
        Task<TagDTO?> CreateTag(TagCreateDTO tagCreateDTO);
        Task<bool> DeleteTag(Guid id);
        Task<IEnumerable<TagDTO>> GetAllTags();
        Task<TagDTO?> GetTagById(Guid id);
        Task<bool> UpdateTag(TagUpdateDTO tagUpdateDTO);
    }
}