using System.Globalization;

namespace Dfe.PersonsApi.Utils.Extensions
{
    public static class DateExtensions
    {
        public static string? ToResponseDate(this DateTime? date)
        {
            if (date == null)
                return null;

            return date.Value.ToString("d", new CultureInfo("en-GB"));
        }
    }
}
