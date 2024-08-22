using SubscriptionManagement.Business.Dtos;
using SubscriptionManagement.Business.Models;

namespace SubscriptionManagement.Business.Mapper
{
    public static class AccountMapper
    {
        public static AccountDto ToDto(this Account customer)
        {
            return new AccountDto(customer.Id.ToString(), customer.Email, customer.Description, customer.CustomerId.ToString());
        }
    }
}
