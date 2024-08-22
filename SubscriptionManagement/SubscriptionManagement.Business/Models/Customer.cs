using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SubscriptionManagement.Business.Models
{
    public class Customer : Entity
    {
        public Customer()
        {
            Accounts = new HashSet<Account>();
        }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Account> Accounts { get; set; }
    }
}
