using SubscriptionManagement.Business.Common.Helpers;

namespace SubscriptionManagement.Business.Dtos
{
    public static class DateTimeMapper
    {
        public static string ToCustomFormattedDateString(this DateTime dateTime, string formatType)
        {
            return formatType switch
            {
                DateFormatConstants.Short => dateTime.ToString("d/M/yyyy"),
                DateFormatConstants.Long => dateTime.ToString("dddd, MMMM d, yyyy"),
                DateFormatConstants.Iso => dateTime.ToString("yyyy-MM-dd"),
                DateFormatConstants.US => dateTime.ToString("M/d/yyyy"),
                _ => dateTime.ToString("d/M/yyyy")
            };
        }
    }
}
