namespace Gamex.Service.Implementation;

public class PictureService : IPictureService
{
    private readonly GamexDbContext _context;

    public PictureService(GamexDbContext context)
    {
        _context = context;
    }

    public async Task<PictureDTO> GetPicture(Guid pictureId)
    {
        var picture = await _context.Pictures.FirstOrDefaultAsync(p => p.Id == pictureId);
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
