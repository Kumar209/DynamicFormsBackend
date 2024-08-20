using DynamicFormsBackend.Models.Dto;
using DynamicFormsBackend.ServiceInterface.FormCreation;
using DynamicFormsBackend.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DynamicFormsBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly ILogger<QuestionController> _logger;
        private readonly IQuestionService _questionService;

        public QuestionController(ILogger<QuestionController> logger, IQuestionService questionService)
        {
            _logger = logger;
            _logger.LogDebug("Nlog is integrated to Form Template controller");

            _questionService = questionService;
        }


        [HttpGet("GetResponseTypes")]
        public async Task<IActionResult> GetResponseTypes()
        {
            try
            {
                var res = await _questionService.GetAnswerTypes();

                if (res != null)
                {
                    return Ok(new { success = true, data = res });
                }

                return StatusCode(500, new { success = false, message = ResponseMessage.internalServerError });
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { success = false, message = ResponseMessage.internalServerError, error = ex.Message });
            }

        }

        [HttpPost("AddQuestion")]
        public async Task<IActionResult> AddQuestion(QuestionDto questionDetails)
        {
            try
            {
                var res = await _questionService.AddQuestion(questionDetails);

                if (res.success)
                {
                    return Ok(new { success = res.success, message = ResponseMessage.questionCreationSuccess, questionId = res.questionId });
                }
                else
                {
                    return StatusCode(500, new { success = res.success, message = ResponseMessage.internalServerError, questionId = res.questionId });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { success = false, message = ResponseMessage.internalServerError, error = ex.Message });
            }
        }



        [HttpGet("GetAllQuestions")]
        public async Task<IActionResult> GetAllQuestions()
        {
            try
            {
                var questions = await _questionService.GetAllQuestions();

                if(questions == null)
                {
                    return StatusCode(500, new { success = false, message = ResponseMessage.internalServerError });
                }

                return Ok(new {success=true, questions = questions});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { success = false, message = ResponseMessage.internalServerError, error = ex.Message });
            }
        }




        [HttpGet("GetQuestionById/{id}")]
        public async Task<IActionResult> GetQuestionById(int id)
        {
            try
            {
                var question = await _questionService.GetQuestion(id);

                if (question == null)
                {
                    return StatusCode(500, new { success = false, message = ResponseMessage.internalServerError });
                }

                return Ok(new { success = true, question = question });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { success = false, message = ResponseMessage.internalServerError, error = ex.Message });
            }
        }


        [HttpDelete("RemoveQuestionById/{id}")]
        public async Task<IActionResult> RemoveQuestionById(int id)
        {
            try
            {
                var res = await _questionService.DeleteQuestionById(id);

                if (res == false)
                {
                    return StatusCode(500, new { success = false, message = ResponseMessage.NotFoundQuestion });
                }

                return Ok(new { success = true, message = ResponseMessage.questionDeleted });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { success = false, message = ResponseMessage.internalServerError, error = ex.Message });
            }
        }




        [HttpPut("UpdateQuestion")]
        public async Task<IActionResult> UpdateQuestion(QuestionDto questionDetails)
        {
            try
            {
                /* var success = await _questionService.UpdateQuestion(questionDetails);*/

                /* if (success)
                 {
                     return Ok(new { success = true, message = "Successfully updated question" });
                 }
                 else
                 {
                     return NotFound(new { success = false, message = "Question not found" });
                 }*/

                return NotFound(new { success = false, message = ResponseMessage.NotFoundQuestion });


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { success = false, message = ResponseMessage.internalServerError, error = ex.Message });
            }
        }



    }
}
