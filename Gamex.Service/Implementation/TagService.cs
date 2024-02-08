using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamex.Service.Implementation;

public class TagService : ITagService
{
    private readonly GamexDbContext _context;

    public TagService(GamexDbContext context)
    {
        _context = context;
    }

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
}
