namespace Gamex.DTO;

public class PictureDTO
{
    public Guid Id { get; set; }
    public string FileUrl { get; set; } = string.Empty;
    public string PublicId { get; set; }
}

public class PictureCreateDTO
{
    public string FileUrl { get; set; } = string.Empty;
    public string PublicId { get; set; }
}

public class PictureUpdateDTO
{
    public Guid Id { get; set; }
    public string FileUrl { get; set; } = string.Empty;
    public string PublicId { get; set; }
}