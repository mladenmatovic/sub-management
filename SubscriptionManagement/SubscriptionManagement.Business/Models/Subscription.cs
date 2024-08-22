using SubscriptionManagement.Business.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionManagement.Business.Models
{
    public class Subscription : Entity
    {
        public string ProductName { get; set; } = default!;
        public string ProductId { get; set; } = default!;
        public string PurchaseUnit { get; set; }
        public int Quantity {  get; set; }
        public SubscriptionState Status { get; set; } 
        public DateTime ValidTo { get; set; }
        public DateTime PreviousValidTo { get; set; }
        public Guid AccountId { get; set; }
        public Account Account { get; set; }        
    }
}
