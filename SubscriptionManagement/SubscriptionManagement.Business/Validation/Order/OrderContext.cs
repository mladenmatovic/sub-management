using SubscriptionManagement.Business.Models;
using SubscriptionManagement.Business.Models.CCP;


namespace SubscriptionManagement.Business.Validation.Order
{
    public class OrderContext
    {
        public Guid AccountId { get; set; }
        public string Email { get; set; }
        public Guid ProductSKU { get; set; }
        public int Amount { get; set; }
        public DateTime ValidTo { get; set; }
        public Account Account { get; set; }
        public CCPProduct Product { get; set; }
        public Guid SubscriptionId { get; set; }
    }
}
