using DynamicFormsBackend.Controllers;
using DynamicFormsBackend.Models.Dto;
using DynamicFormsBackend.ServiceInterface.FormCreation;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DynamicFormsBackend.Test.Controllers
{
    public class FormTemplateControllerTests
    {
        private MockRepository mockRepository;

        private Mock<ILogger<FormTemplateController>> mockLogger;
        private Mock<IFormService> mockFormService;

        public FormTemplateControllerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockLogger = this.mockRepository.Create<ILogger<FormTemplateController>>();
            this.mockFormService = this.mockRepository.Create<IFormService>();
        }

        private FormTemplateController CreateFormTemplateController()
        {
            return new FormTemplateController(
                this.mockLogger.Object,
                this.mockFormService.Object);
        }

        [Fact]
        public async Task CreateTemplate_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var formTemplateController = this.CreateFormTemplateController();
            SourceTemplateDto templateDto = null;

            // Act
            var result = await formTemplateController.CreateTemplate(
                templateDto);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetAllForms_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var formTemplateController = this.CreateFormTemplateController();

            // Act
            var result = await formTemplateController.GetAllForms();

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetFormById_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var formTemplateController = this.CreateFormTemplateController();
            int formId = 0;

            // Act
            var result = await formTemplateController.GetFormById(
                formId);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task RemoveFormById_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var formTemplateController = this.CreateFormTemplateController();
            int formId = 0;

            // Act
            var result = await formTemplateController.RemoveFormById(
                formId);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task UpdateTemplate_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var formTemplateController = this.CreateFormTemplateController();
            int formId = 0;
            SourceTemplateDto templateDto = null;

            // Act
            var result = await formTemplateController.UpdateTemplate(
                formId,
                templateDto);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }
    }
}
