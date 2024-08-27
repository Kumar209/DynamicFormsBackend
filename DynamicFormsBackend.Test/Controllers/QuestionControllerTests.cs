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
    public class QuestionControllerTests
    {
        private MockRepository mockRepository;

        private Mock<ILogger<QuestionController>> mockLogger;
        private Mock<IQuestionService> mockQuestionService;

        public QuestionControllerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockLogger = this.mockRepository.Create<ILogger<QuestionController>>();
            this.mockQuestionService = this.mockRepository.Create<IQuestionService>();
        }

        private QuestionController CreateQuestionController()
        {
            return new QuestionController(
                this.mockLogger.Object,
                this.mockQuestionService.Object);
        }

        [Fact]
        public async Task GetResponseTypes_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var questionController = this.CreateQuestionController();

            // Act
            var result = await questionController.GetResponseTypes();

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task AddQuestion_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var questionController = this.CreateQuestionController();
            QuestionDto questionDetails = null;

            // Act
            var result = await questionController.AddQuestion(
                questionDetails);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetAllQuestions_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var questionController = this.CreateQuestionController();

            // Act
            var result = await questionController.GetAllQuestions();

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetQuestionById_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var questionController = this.CreateQuestionController();
            int id = 0;

            // Act
            var result = await questionController.GetQuestionById(
                id);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task RemoveQuestionById_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var questionController = this.CreateQuestionController();
            int id = 0;

            // Act
            var result = await questionController.RemoveQuestionById(
                id);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task UpdateQuestion_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var questionController = this.CreateQuestionController();
            QuestionDto questionDetails = null;

            // Act
            var result = await questionController.UpdateQuestion(
                questionDetails);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }


    }
}
