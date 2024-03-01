namespace Gamex.Service.Implementation;

public class PictureService(GamexDbContext context) : IPictureService
{
    private readonly GamexDbContext _context = context;

    /// <summary>
    /// Retrieves a picture by its ID.
    /// </summary>
    /// <param name="pictureId">The ID of the picture.</param>
    /// <returns>The picture DTO if found, otherwise null.</returns>
    public async Task<PictureDTO> GetPicture(Guid pictureId)
    {
        var picture = await _context.Pictures.AsNoTracking().FirstOrDefaultAsync(p => p.Id == pictureId);
        if (picture == null)
        {
            return null;
        }
        return new PictureDTO(picture.Id, picture.FileUrl, picture.PublicId);
    }

    /// <summary>
    /// Retrieves a picture by its public ID.
    /// </summary>
    /// <param name="publicId">The public ID of the picture.</param>
    /// <returns>The picture DTO if found, otherwise null.</returns>
    public async Task<PictureDTO> GetPictureByPublicId(string publicId)
    {
        var picture = await _context.Pictures.AsNoTracking().FirstOrDefaultAsync(p => p.PublicId == publicId);
        if (picture == null)
        {
            return null;
        }
        return new PictureDTO(picture.Id, picture.FileUrl, picture.PublicId);
    }

    /// <summary>
    /// Creates a new picture.
    /// </summary>
    /// <param name="pictureCreateDTO">The picture create DTO.</param>
    /// <returns>The created picture DTO.</returns>
    public async Task<PictureDTO> CreatePicture(PictureCreateDTO pictureCreateDTO)
    {
        var picture = new Picture()
        {
            FileUrl = pictureCreateDTO.FileUrl,
            PublicId = pictureCreateDTO.PublicId
        };
        await _context.Pictures.AddAsync(picture);
        await _context.SaveChangesAsync();
        return new PictureDTO(picture.Id, picture.FileUrl, picture.PublicId);
    }

    /// <summary>
    /// Updates an existing picture.
    /// </summary>
    /// <param name="pictureUpdateDTO">The picture update DTO.</param>
    /// <returns>The updated picture DTO if found, otherwise null.</returns>
    public async Task<PictureDTO> UpdatePicture(PictureUpdateDTO pictureUpdateDTO)
    {
        var picture = await _context.Pictures.FirstOrDefaultAsync(p => p.Id == pictureUpdateDTO.Id);
        if (picture == null)
        {
            return null;
        }
        picture.FileUrl = pictureUpdateDTO.FileUrl;
        picture.PublicId = pictureUpdateDTO.PublicId;
        await _context.SaveChangesAsync();
        return new PictureDTO(picture.Id, picture.FileUrl, picture.PublicId);
    }

    /// <summary>
    /// Deletes a picture by its ID.
    /// </summary>
    /// <param name="pictureId">The ID of the picture.</param>
    /// <returns>True if the picture was deleted successfully, otherwise false.</returns>
    public async Task<bool> DeletePicture(Guid pictureId)
    {
        var picture = await _context.Pictures.FirstOrDefaultAsync(p => p.Id == pictureId);
        if (picture == null)
        {
            return false;
        }
        _context.Pictures.Remove(picture);
        await _context.SaveChangesAsync();
        return true;
    }
}
