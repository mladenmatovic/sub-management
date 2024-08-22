using System;
using System.Collections.Generic;
using System.Linq;
namespace SubscriptionManagement.Business.Models
{
    public class Account : Entity
    {      
        public Account() 
        {
            Subscriptions = new HashSet<Subscription>();
        }
        public string Email { get; set; } = default!;
        public string Description { get; set; } = default!;
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }

        public ICollection<Subscription> Subscriptions { get; set; }
    }
}
