using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;

namespace Gamex.Common;

public static class CommonHelpers
{
    /// <summary>
    /// Convert string to date. Format: yyyy/MM/dd
    /// </summary>
    /// <param name="date">The string representation of the date.</param>
    /// <returns>The converted DateTime object.</returns>
    public static DateTime ConvertToDate(string date)
    {
        var splitedDate = date.Split('/');

        DateTime convertedDate = new(int.Parse(splitedDate[2]), int.Parse(splitedDate[1]), int.Parse(splitedDate[0]));
        return convertedDate;
    }

    /// <summary>
    /// Convert string to time. Format: HH:mm
    /// </summary>
    /// <param name="time">The string representation of the time.</param>
    /// <returns>The converted DateTime object.</returns>
    public static DateTime ConvertToTime(string time)
    {
        var splittedTime = time.Split(':');
        DateTime convertedTime = new(1, 1, 1, int.Parse(splittedTime[0]), int.Parse(splittedTime[1]), 0);
        return convertedTime;
    }

    /// <summary>
    /// Check the file size of an IFormFile.
    /// </summary>
    /// <param name="file">The IFormFile to check.</param>
    /// <param name="maxSize">The maximum allowed file size.</param>
    /// <returns>A tuple indicating whether the file size is valid and a corresponding message.</returns>
    public static (bool, string) CheckFileSize(IFormFile file, long maxSize)
    {
        if (file.Length > maxSize)
        {
            return (false, "File size is too large");
        }
        return (true, "File size is valid");
    }

    /// <summary>
    /// Check the file size of an IBrowserFile.
    /// </summary>
    /// <param name="file">The IBrowserFile to check.</param>
    /// <param name="maxSize">The maximum allowed file size.</param>
    /// <returns>A tuple indicating whether the file size is valid and a corresponding message.</returns>
    public static (bool, string) CheckFileSize(IBrowserFile file, long maxSize)
    {
        if (file.Size > maxSize)
        {
            return (false, "File size is too large");
        }
        return (true, "File size is valid");
    }

    /// <summary>
    /// Check the file extension of an IFormFile.
    /// </summary>
    /// <param name="file">The IFormFile to check.</param>
    /// <param name="validExtensions">An array of valid file extensions.</param>
    /// <returns>A tuple indicating whether the file extension is valid and a corresponding message.</returns>
    public static (bool, string) CheckFileExtension(IFormFile file, string[] validExtensions)
    {
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (string.IsNullOrEmpty(fileExtension) || !validExtensions.Contains(fileExtension))
        {
            return (false, "File extension is not valid");
        }
        return (true, "File extension is valid");
    }

    /// <summary>
    /// Check the file extension of an IBrowserFile.
    /// </summary>
    /// <param name="file">The IBrowserFile to check.</param>
    /// <param name="validExtensions">An array of valid file extensions.</param>
    /// <returns>A tuple indicating whether the file extension is valid and a corresponding message.</returns>
    public static (bool, string) CheckFileExtension(IBrowserFile file, string[] validExtensions)
    {
        var fileExtension = Path.GetExtension(file.Name).ToLowerInvariant();
        if (string.IsNullOrEmpty(fileExtension) || !validExtensions.Contains(fileExtension))
        {
            return (false, "File extension is not valid");
        }
        return (true, "File extension is valid");
    }

    /// <summary>
    /// Generate a random string.
    /// </summary>
    /// <param name="length">The length of the random string.</param>
    /// <param name="uppercase">Indicates whether the random string should contain uppercase letters.</param>
    /// <param name="specialXter">Indicates whether the random string should contain special characters.</param>
    /// <returns>The generated random string.</returns>
    public static string GenerateRandomString(int length, bool uppercase = false, bool specialXter = false)
    {
        RandomStringGenerator stringGenerator = new();

        return stringGenerator.NextString(length, true, uppercase, true, specialXter);
    }

    /// <summary>
    /// Generate a random number.
    /// </summary>
    /// <param name="length">The number of digits in the random number.</param>
    /// <returns>The generated random number.</returns>
    public static int GenerateRandomNumbers(int length)
    {
        Random random = new();
        int minValue = (int)Math.Pow(10, length - 1);
        int maxValue = (int)Math.Pow(10, length) - 1;
        int randomNumber = random.Next(minValue, maxValue);
        return randomNumber;
    }
}
