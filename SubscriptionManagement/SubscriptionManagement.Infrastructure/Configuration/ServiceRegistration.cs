using Microsoft.Extensions.DependencyInjection;
using SubscriptionManagement.Business.Repositories;
using SubscriptionManagement.Infrastructure.Repositories;

namespace SubscriptionManagement.Infrastructure.Configuration
{
    public static class ServiceRegistration
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
                services.AddScoped<ICustomerRepository, CustomerRepository>();
                services.AddScoped<IAccountRepository, AccountRepository>();
                services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
                services.AddScoped<ICloudPartnerRepository, CloudPartnerRepository>();

                return services;
        }
    }
}
