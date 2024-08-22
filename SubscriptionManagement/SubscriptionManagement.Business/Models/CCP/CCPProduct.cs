using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionManagement.Business.Models.CCP
{
    public class CCPProduct
    {
        public Guid SKU { get; set; }
        public int ProductId { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string PurchaseUnit { get; set; }
        public decimal Price { get; set; }
    }

}
