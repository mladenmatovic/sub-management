using SubscriptionManagement.Business.Models.CCP;

namespace SubscriptionManagement.Infrastructure.Repositories
{
    public class SubscriptionResponse
    {
        public string SubstrictionId { get; set; }
        public string Status { get; set; }
        public CCPProduct? Product { get; set; }
    }
}