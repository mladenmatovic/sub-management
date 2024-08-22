using Microsoft.Extensions.DependencyInjection;
using SubscriptionManagement.Business.Services;


namespace SubscriptionManagement.Business.Configuration
{
    public static class ServiceRegistration
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductCatalogService, ProductCatalogService>();
            
            return services;
        }
    }
}
