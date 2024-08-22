namespace SubscriptionManagement.Business.Commands
{
    public class OrderCancelCommand
    {
        public Guid SubscriptionId { get; set; }
        public string AccountEmail { get; set; }
        public Guid ProductSKU { get; set; }
    }
}
