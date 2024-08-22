using SubscriptionManagement.Business.Dtos;
using SubscriptionManagement.Business.Models;

namespace SubscriptionManagement.Business.Mapper
{
    public static class CustomerMapper
    {
        public static CustomerDto ToDto(this Customer customer)
        {
            return new CustomerDto(customer.Id.ToString(), customer.Name, customer.IsActive);
        }
    }
}
