using SubscriptionManagement.Business.Repositories;
using SubscriptionManagement.Business.Services;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SubscriptionManagement.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<string> GetJsonDocumentAsync(Guid tenantId, Guid documentId)
        {
            return string.Empty;
        }
    }
}
