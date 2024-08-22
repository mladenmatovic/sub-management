using Moq;
using SubscriptionManagement.Business.Models.CCP;
using SubscriptionManagement.Business.Models;
using SubscriptionManagement.Business.Repositories;
using SubscriptionManagement.Business.Services;
using Microsoft.Extensions.Logging;
using SubscriptionManagement.Infrastructure.Repositories;


namespace SubscriptionManagement.UnitTests
{
    public class OrderServiceTests
    {
        private readonly Mock<IAccountRepository> _mockAccountRepo;
        private readonly Mock<ISubscriptionRepository> _mockSubscriptionRepo;
        private readonly Mock<ICloudPartnerRepository> _mockCloudPartnerRepo;
        private readonly Mock<ILogger<OrderService>> _mockLogger;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _mockAccountRepo = new Mock<IAccountRepository>();
            _mockSubscriptionRepo = new Mock<ISubscriptionRepository>();
            _mockCloudPartnerRepo = new Mock<ICloudPartnerRepository>();
            _mockLogger = new Mock<ILogger<OrderService>>();

            _orderService = new OrderService(
                _mockAccountRepo.Object,
                _mockSubscriptionRepo.Object,
                _mockCloudPartnerRepo.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task OrderNew_SuccessfulOrder_ReturnsTrue()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var email = "test@example.com";
            var productSKU = Guid.NewGuid();
            var amount = 1;
            var validTo = DateTime.Now.AddDays(30);

            _mockAccountRepo.Setup(repo => repo.GetByIdAsync(accountId))
                .ReturnsAsync(new Account { /* populate with necessary data */ });

            _mockCloudPartnerRepo.Setup(repo => repo.GetProductBySKUAsync(productSKU))
                .ReturnsAsync(new CCPProduct { /* populate with necessary data */ });

            _mockCloudPartnerRepo.Setup(repo => repo.OrderSubscriptionAsync(email, productSKU, amount))
                .ReturnsAsync(new SubscriptionResponse { Status = Status.SUCCESS });

            // Act
            var result = await _orderService.OrderNew(accountId, email, productSKU, amount, validTo);

            // Assert
            Assert.True(result.Match(success => success, error => false));
        }

        [Fact]
        public async Task OrderNew_AccountNotFound_ReturnsError()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var email = "test@example.com";
            var productSKU = Guid.NewGuid();
            var amount = 1;
            var validTo = DateTime.Now.AddDays(30);

            _mockAccountRepo.Setup(repo => repo.GetByIdAsync(accountId))
                .ReturnsAsync((Account)null);

            // Act
            var result = await _orderService.OrderNew(accountId, email, productSKU, amount, validTo);

            // Assert
            Assert.True(result.Match(
                success => false,
                error => error.Code == "OrderAdd.BadAccount"
            ));
        }

        [Fact]
        public async Task OrderNew_CloudPartnerAPIFail_ReturnsError()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var email = "test@example.com";
            var productSKU = Guid.NewGuid();
            var amount = 1;
            var validTo = DateTime.Now.AddDays(30);

            _mockAccountRepo.Setup(repo => repo.GetByIdAsync(accountId))
                .ReturnsAsync(new Account { });

            _mockCloudPartnerRepo.Setup(repo => repo.GetProductBySKUAsync(productSKU))
                .ReturnsAsync(new CCPProduct {  });

            _mockCloudPartnerRepo.Setup(repo => repo.OrderSubscriptionAsync(email, productSKU, amount))
                .ReturnsAsync(new SubscriptionResponse { Status = Status.ERROR });

            // Act
            var result = await _orderService.OrderNew(accountId, email, productSKU, amount, validTo);

            // Assert
            Assert.True(result.Match(
                success => false,
                error => error.Code == "OrderAdd.CloudAPIFail"
            ));
        }
    }
}
