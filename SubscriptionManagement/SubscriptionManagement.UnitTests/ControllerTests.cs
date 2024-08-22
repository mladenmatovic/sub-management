using Microsoft.AspNetCore.Mvc;
using Moq;
using SubscriptionManagement.Api.Controllers;
using SubscriptionManagement.Business.Commands;
using SubscriptionManagement.Business.Common;
using SubscriptionManagement.Business.Dtos;
using SubscriptionManagement.Business.Models;
using SubscriptionManagement.Business.Services;


namespace SubscriptionManagement.UnitTests
{
    public class ControllerTests
    {
        private readonly Mock<ICustomerService> _mockCustomerService;
        private readonly CustomersController _controller;

        public ControllerTests()
        {
            _mockCustomerService = new Mock<ICustomerService>();
            _controller = new CustomersController(_mockCustomerService.Object);
        }

        [Fact]
        public async Task Get_ReturnsOkResultWithCustomers_WhenCustomersExist()
        {
            // Arrange
            var customers = new List<Customer>
        {
            new Customer { Id = Guid.NewGuid(), Name = "Test Customer" },
            new Customer { Id = Guid.NewGuid(), Name = "Another Customer" }
        };
            _mockCustomerService.Setup(s => s.GetCustomers()).ReturnsAsync(customers);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<CustomerDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count());
        }

        [Fact]
        public async Task Get_ReturnsForbiddenResult_WhenNoCustomersFound()
        {
            // Arrange
            _mockCustomerService.Setup(s => s.GetCustomers()).ReturnsAsync((List<Customer>)null);

            // Act
            var result = await _controller.Get();

            // Assert
            Assert.IsType<StatusCodeResult>(result);
            var statusCodeResult = (StatusCodeResult)result;
            Assert.Equal(403, statusCodeResult.StatusCode);
        }
    }
}
