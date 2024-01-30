namespace Gamex.Service.Implementation;

public class PictureService(GamexDbContext context) : IPictureService
{
    private readonly GamexDbContext _context = context;

    public async Task<PictureDTO> GetPicture(Guid pictureId)
    {
        var picture = await _context.Pictures.AsNoTracking().FirstOrDefaultAsync(p => p.Id == pictureId);
        if (picture == null)
        {
            return null;
        }
        return new PictureDTO(picture.Id, picture.FileUrl, picture.PublicId);
    }

    public async Task<PictureDTO> GetPictureByPublicId(string publicId)
    {
        var picture = await _context.Pictures.AsNoTracking().FirstOrDefaultAsync(p => p.PublicId == publicId);
        if (picture == null)
        {
            return null;
        }
        return new PictureDTO(picture.Id, picture.FileUrl, picture.PublicId);
    }

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

    public async Task<PictureDTO> CreatePictureForUser(PictureCreateDTO pictureCreateDTO, string userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            return null;
        }
        var picture = new Picture()
        {
            FileUrl = pictureCreateDTO.FileUrl,
            PublicId = pictureCreateDTO.PublicId
        };
        await _context.Pictures.AddAsync(picture);
        await _context.SaveChangesAsync();
        return new PictureDTO(picture.Id, picture.FileUrl, picture.PublicId);
    }

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
