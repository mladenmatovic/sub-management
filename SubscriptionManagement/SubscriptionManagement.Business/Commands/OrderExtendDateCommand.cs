namespace SubscriptionManagement.Business.Commands
{
    public class OrderExtendDateCommand
    {
        public Guid SubscriptionId { get; set; }
        public string AccountEmail { get; set; }
        public Guid ProductSKU { get; set; }
        public string ValidTo { get; set; }
    }
}
