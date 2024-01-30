using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;

namespace Gamex.Common;

public static class CommonHelpers
{
    /// <summary>
    /// Convert string to date. Format: yyyy/MM/dd
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static DateTime ConvertToDate(string date)
    {
        var splitedDate = date.Split('/');

        DateTime convertedDate = new(int.Parse(splitedDate[2]), int.Parse(splitedDate[1]), int.Parse(splitedDate[0]));
        return convertedDate;
    }

    /// <summary>
    /// Convert string to time. Format: HH:mm
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static DateTime ConvertToTime(string time)
    {
        var splittedTime = time.Split(':');
        DateTime convertedTime = new(1, 1, 1, int.Parse(splittedTime[0]), int.Parse(splittedTime[1]), 0);
        return convertedTime;
    }

    public static (bool, string) CheckFileSize(IFormFile file, long maxSize)
    {
        if (file.Length > maxSize)
        {
            return (false, "File size is too large");
        }
        return (true, "File size is valid");
    }

    public static (bool, string) CheckFileSize(IBrowserFile file, long maxSize)
    {
        if (file.Size > maxSize)
        {
            return (false, "File size is too large");
        }
        return (true, "File size is valid");
    }

    public static (bool, string) CheckFileExtension(IFormFile file, string[] validExtensions)
    {
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (string.IsNullOrEmpty(fileExtension) || !validExtensions.Contains(fileExtension))
        {
            return (false, "File extension is not valid");
        }
        return (true, "File extension is valid");
    }

    public static (bool, string) CheckFileExtension(IBrowserFile file, string[] validExtensions)
    {
        var fileExtension = Path.GetExtension(file.Name).ToLowerInvariant();
        if (string.IsNullOrEmpty(fileExtension) || !validExtensions.Contains(fileExtension))
        {
            return (false, "File extension is not valid");
        }
        return (true, "File extension is valid");
    }
}
