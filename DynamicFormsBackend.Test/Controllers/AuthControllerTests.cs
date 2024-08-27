using DynamicFormsBackend.Controllers;
using DynamicFormsBackend.Models.Dto;
using DynamicFormsBackend.ServiceInterface.Authentication;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DynamicFormsBackend.Test.Controllers
{
    public class AuthControllerTests
    {
        private MockRepository mockRepository;

        private Mock<ILogger<AuthController>> mockLogger;
        private Mock<IAuthService> mockAuthService;

        public AuthControllerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockLogger = this.mockRepository.Create<ILogger<AuthController>>();
            this.mockAuthService = this.mockRepository.Create<IAuthService>();
        }

        private AuthController CreateAuthController()
        {
            return new AuthController(
                this.mockLogger.Object,
                this.mockAuthService.Object);
        }

        [Fact]
        public async Task login_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var authController = this.CreateAuthController();
            LoginCredential credential = null;

            // Act
            var result = await authController.login(
                credential);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }
    }
}
