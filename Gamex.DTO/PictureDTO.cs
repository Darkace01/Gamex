namespace Gamex.DTO;

public class PictureDTO
{
    public Guid Id { get; set; }
    public string FileUrl { get; set; } = string.Empty;
    public string PublicId { get; set; }

    public PictureDTO(Guid id, string fileUrl, string publicId)
    {
        Id = id;
        FileUrl = fileUrl;
        PublicId = publicId;
    }
}

public class PictureCreateDTO
{
    public string FileUrl { get; set; } = string.Empty;
    public string PublicId { get; set; }

    public PictureCreateDTO(string fileUrl, string publicId)
    {
        FileUrl = fileUrl;
        PublicId = publicId;
    }
}

public class PictureUpdateDTO
{
    public Guid Id { get; set; }
    public string FileUrl { get; set; } = string.Empty;
    public string PublicId { get; set; }

    public PictureUpdateDTO(Guid id, string fileUrl, string publicId)
    {
        Id = id;
        FileUrl = fileUrl;
        PublicId = publicId;
    }
}