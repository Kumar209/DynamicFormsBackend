using DynamicFormsBackend.Controllers;
using DynamicFormsBackend.Models.Dto;
using DynamicFormsBackend.ServiceInterface.Response;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DynamicFormsBackend.Test.Controllers
{
    public class FormResponseControllerTests
    {
        private MockRepository mockRepository;

        private Mock<ILogger<FormResponseController>> mockLogger;
        private Mock<IFormResponseService> mockFormResponseService;

        public FormResponseControllerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockLogger = this.mockRepository.Create<ILogger<FormResponseController>>();
            this.mockFormResponseService = this.mockRepository.Create<IFormResponseService>();
        }

        private FormResponseController CreateFormResponseController()
        {
            return new FormResponseController(
                this.mockLogger.Object,
                this.mockFormResponseService.Object);
        }

        [Fact]
        public async Task InsertResponse_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var formResponseController = this.CreateFormResponseController();
            FormResponseDto response = null;

            // Act
            var result = await formResponseController.InsertResponse(
                response);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetAllResponses_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var formResponseController = this.CreateFormResponseController();
            int formId = 0;

            // Act
            var result = await formResponseController.GetAllResponses(
                formId);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetResponseById_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var formResponseController = this.CreateFormResponseController();
            int id = 0;

            // Act
            var result = await formResponseController.GetResponseById(
                id);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task DeleteResponse_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var formResponseController = this.CreateFormResponseController();
            int id = 0;

            // Act
            var result = await formResponseController.DeleteResponse(
                id);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }
    }
}
