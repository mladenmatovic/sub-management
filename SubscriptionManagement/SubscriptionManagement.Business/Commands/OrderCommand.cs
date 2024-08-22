using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionManagement.Business.Commands
{
    public class OrderCommand
    {
        public Guid AccountId { get; set; }
        public string AccountEmail { get; set; }
        public Guid ProductSKU { get; set; }
        public int Amount { get; set; }
        public string ValidTo { get; set; }
    }
}
