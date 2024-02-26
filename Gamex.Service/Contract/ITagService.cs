namespace Gamex.Service.Contract
{
    public interface ITagService
    {
        Task<TagDTO?> CreateTag(TagCreateDTO tagCreateDTO);
        Task<bool> DeleteTag(Guid id);
        IQueryable<TagDTO> GetAllTags();
        Task<TagDTO?> GetTagById(Guid id);
        Task<TagDTO> GetTagByName(string name);
        Task<bool> UpdateTag(TagUpdateDTO tagUpdateDTO);
    }
}