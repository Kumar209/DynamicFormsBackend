using DynamicFormsBackend.Controllers;
using DynamicFormsBackend.Models.Dto;
using DynamicFormsBackend.Models.Entities;
using DynamicFormsBackend.ServiceInterface.FormCreation;
using Microsoft.AspNetCore.Mvc;
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
            /* var formTemplateController = this.CreateFormTemplateController();

             // Act
             var result = await formTemplateController.GetAllForms();

             // Assert
             Assert.True(false);
             this.mockRepository.VerifyAll();*/



            // Arrange
            var expectedForms = new List<SourceTemplateDto>
            {
                new SourceTemplateDto { Id = 1, FormName = "Form 1", Description = "Form 1 description" , IsPublish = true, Version = 1 },
                new SourceTemplateDto { Id = 2, FormName = "Form 2", Description = "Form 2 description", IsPublish = false, Version = 1 },
                new SourceTemplateDto { Id = 3, FormName = "Form 3", Description = "Form 3 description", IsPublish = true, Version = 1 }
            };

            mockFormService.Setup(fs => fs.Getforms()).Returns(Task.FromResult(
                expectedForms.Select(x => new SourceTemplate { Id = x.Id ?? 0, FormName = x.FormName, Description = x.Description, IsPublish = x.IsPublish, Version = x.Version })
            ));

            var formTemplateController = CreateFormTemplateController();

            // Act
            var result = await formTemplateController.GetAllForms();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);

            var actualForms = okResult.Value as List<SourceTemplateDto>;
            Assert.NotNull(actualForms);

            Assert.Equal(expectedForms.Count, actualForms.Count);

            for (int i = 0; i < expectedForms.Count; i++)
            {
                Assert.Equal(expectedForms[i].Id, actualForms[i].Id);
                Assert.Equal(expectedForms[i].FormName, actualForms[i].FormName);
                Assert.Equal(expectedForms[i].Description, actualForms[i].Description);
                Assert.Equal(expectedForms[i].IsPublish, actualForms[i].IsPublish);
                Assert.Equal(expectedForms[i].Version, actualForms[i].Version);
            }

            mockFormService.Verify(fs => fs.Getforms(), Times.Once);
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
