using Microsoft.Extensions.Logging;
using SubscriptionManagement.Business.Models;
using SubscriptionManagement.Business.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionManagement.Business.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(
            ICustomerRepository customerRepository,
            ILogger<CustomerService> logger)
        {
            _customerRepository = customerRepository;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            try
            {
                var isSupported = await _customerRepository.GetAll();
                _logger.LogInformation($"Getting all customer in the system");
                return isSupported;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error Getting all customer in the system.");
                throw;
            }
        }
    }
}
