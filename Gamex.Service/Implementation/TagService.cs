using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamex.Service.Implementation;

/// <summary>
/// Service class for managing tags.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="TagService"/> class.
/// </remarks>
/// <param name="context">The database context.</param>
public class TagService(GamexDbContext context) : ITagService
{
    private readonly GamexDbContext _context = context;

    /// <summary>
    /// Gets all tags.
    /// </summary>
    /// <returns>The collection of all tags.</returns>
    public IQueryable<TagDTO> GetAllTags()
    {
        var tags = _context.Tags.Include(t => t.PostTags)
            .AsNoTracking()
            .Select(t => new TagDTO
            {
                Id = t.Id,
                Name = t.Name,
                PostCount = t.PostTags.Count
            });

        return tags;
    }

    /// <summary>
    /// Gets a tag by its ID.
    /// </summary>
    /// <param name="id">The ID of the tag.</param>
    /// <returns>The tag with the specified ID, or null if not found.</returns>
    public async Task<TagDTO?> GetTagById(Guid id)
    {
        var tag = await _context.Tags.Include(t => t.PostTags)
            .AsNoTracking()
            .Select(t => new TagDTO
            {
                Id = t.Id,
                Name = t.Name,
                PostCount = t.PostTags.Count
            })
            .FirstOrDefaultAsync(t => t.Id == id);

        return tag;
    }

    /// <summary>
    /// Creates a new tag.
    /// </summary>
    /// <param name="tagCreateDTO">The DTO containing the tag information.</param>
    /// <returns>The created tag.</returns>
    public async Task<TagDTO?> CreateTag(TagCreateDTO tagCreateDTO)
    {
        var tag = new Tag
        {
            Name = tagCreateDTO.Name
        };

        await _context.Tags.AddAsync(tag);
        await _context.SaveChangesAsync();

        return new TagDTO
        {
            Id = tag.Id,
            Name = tag.Name,
            PostCount = 0
        };
    }

    /// <summary>
    /// Updates a tag.
    /// </summary>
    /// <param name="tagUpdateDTO">The DTO containing the updated tag information.</param>
    /// <returns>True if the tag was updated successfully, false otherwise.</returns>
    public async Task<bool> UpdateTag(TagUpdateDTO tagUpdateDTO)
    {
        var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == tagUpdateDTO.Id);

        if (tag == null)
        {
            return false;
        }

        tag.Name = tagUpdateDTO.Name;

        await _context.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Deletes a tag by its ID.
    /// </summary>
    /// <param name="id">The ID of the tag to delete.</param>
    /// <returns>True if the tag was deleted successfully, false otherwise.</returns>
    public async Task<bool> DeleteTag(Guid id)
    {
        var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);

        if (tag == null)
        {
            return false;
        }

        _context.Tags.Remove(tag);
        await _context.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Gets a tag by its name.
    /// </summary>
    /// <param name="name">The name of the tag.</param>
    /// <returns>The tag with the specified name.</returns>
    public async Task<TagDTO> GetTagByName(string name)
    {
        var tag = await _context.Tags.Include(t => t.PostTags)
            .AsNoTracking()
            .Select(t => new TagDTO
            {
                Id = t.Id,
                Name = t.Name,
                PostCount = t.PostTags.Count
            })
            .FirstOrDefaultAsync(t => t.Name == name);

        return tag;
    }
}
