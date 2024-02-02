using Gamex.Common;

namespace Gamex.DTO;

public class TournamentDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsFeatured { get; set; } = false;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string StartDateFormatted { get { return StartDate?.ToString("dd/MM/yyyy"); } }
    public string EndDateFormatted { get { return EndDate?.ToString("dd/MM/yyyy"); } }
    public string Location { get; set; } = string.Empty;
    public DateTime? Time { get; set; }
    public string TimeFormatted { get { return Time?.ToString("HH:mm"); } }
    public decimal EntryFee { get; set; }
    public string Rules { get; set; } = string.Empty;
    public Guid? PictureId { get; set; }
    public string PictureUrl { get; set; } = string.Empty;
    public string PicturePublicId { get; set; } = string.Empty;
    public IEnumerable<TournamentCategoryDTO> Categories { get; set; }
    public IEnumerable<TournamentUserDTO> TournamentUsers { get; set; }
}

public class PaginatedTournamentDTO
{
    public IEnumerable<TournamentDTO> Tournaments { get; set; }
    public decimal TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
}

public class TournamentCreateDTO
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsFeatured { get; set; } = false;
    public DateTime? StartDate
    {
        get
        {
            return string.IsNullOrEmpty(StartDateString)
                ? null
                : StartDateString.Contains("/") ? CommonHelpers.ConvertToDate(StartDateString) : null;
        }
    }
    public DateTime? StartDateForm { get; set; }
    public string StartDateString { get; set; } = string.Empty;
    public DateTime? EndDate
    {
        get
        {
            return string.IsNullOrEmpty(EndDateString)
                ? null
                : EndDateString.Contains("/") ? CommonHelpers.ConvertToDate(EndDateString) : null;
        }
    }
    public DateTime? EndDateForm { get; set; }
    public string EndDateString { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime? Time
    {
        get
        {
            return string.IsNullOrEmpty(TimeString)
                ? null
                : TimeString.Contains(":") ? CommonHelpers.ConvertToTime(TimeString) : null;
        }
    }
    public DateTime? TimeForm { get; set; }
    public string TimeString { get; set; } = string.Empty;
    public decimal EntryFee { get; set; }
    public string Rules { get; set; } = string.Empty;
    public Guid? PictureId { get; set; }
}

public class TournamentUpdateDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsFeatured { get; set; } = false;
    public DateTime? StartDate
    {
        get
        {
            return string.IsNullOrEmpty(StartDateString)
                ? null
                : StartDateString.Contains("/") ? CommonHelpers.ConvertToDate(StartDateString) : null;
        }
    }
    public DateTime? StartDateForm { get; set; }
    public string StartDateString { get; set; } = string.Empty;
    public DateTime? EndDate
    {
        get
        {
            return string.IsNullOrEmpty(EndDateString)
                ? null
                : EndDateString.Contains("/") ? CommonHelpers.ConvertToDate(EndDateString) : null;
        }
    }
    public DateTime? EndDateForm { get; set; }
    public string EndDateString { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime? Time
    {
        get
        {
            return string.IsNullOrEmpty(TimeString)
                ? null
                : TimeString.Contains(":") ? CommonHelpers.ConvertToTime(TimeString) : null;
        }
    }
    public string TimeString { get; set; } = string.Empty;
    public decimal EntryFee { get; set; }
    public string Rules { get; set; } = string.Empty;
    public Guid? PictureId { get; set; }
}
