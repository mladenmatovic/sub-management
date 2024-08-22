using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionManagement.Business.Commands
{
    public class OrderChangeQuantityCommand
    {
        public Guid SubscriptionId { get; set; }
        public string AccountEmail { get; set; }
        public Guid ProductSKU { get; set; }
        public int Amount { get; set; }
    }
}
