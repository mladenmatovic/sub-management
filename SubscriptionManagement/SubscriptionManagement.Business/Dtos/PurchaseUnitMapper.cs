namespace SubscriptionManagement.Business.Dtos
{
    public static class PurchaseUnitMapper
    {
        public static string ToFriendlyString(string purchaseUnit)
        {
            return purchaseUnit switch
            {
                "1M" => "1 Month",
                "3M" => "3 Months",
                "1W" => "1 Week",
                "1Y" => "1 Year",
                _ => purchaseUnit // If no match, return the original value
            };
        }
    }
}
