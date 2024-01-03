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
}
