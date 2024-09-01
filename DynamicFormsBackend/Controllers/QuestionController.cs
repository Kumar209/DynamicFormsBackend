using DynamicFormsBackend.Models.Dto;
using DynamicFormsBackend.ServiceInterface;
using DynamicFormsBackend.ServiceInterface.FormCreation;
using DynamicFormsBackend.Shared;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IJwtService _jwtService;

        public QuestionController(ILogger<QuestionController> logger, IQuestionService questionService, IJwtService jwtService)
        {
            _logger = logger;
            _logger.LogDebug("Nlog is integrated to Form Template controller");

            _questionService = questionService;
            _jwtService = jwtService;
        }



        /// <summary>
        /// API to get list of response type on basis of user can create question , like radio, text, dropdown, etc
        /// </summary>
        /// <returns>List of response type</returns>

        [HttpGet("GetResponseTypes")]
        [Authorize]
        public async Task<IActionResult> GetResponseTypes()
        {
            try
            {
                var authHeader = Request.Headers["Authorization"].FirstOrDefault();

                if (authHeader == null || !authHeader.StartsWith("Bearer "))
                {
                    _logger.LogWarning(ResponseMessage.unauthorizedAttempt);

                    return Unauthorized(new { success = false, message = ResponseMessage.unauthorizeUser });
                }

                var token = authHeader.Substring("Bearer ".Length).Trim();

                //Validate token condtion if we have to use jwt
                var claims = await _jwtService.ValidateJwtToken(token);

                if (claims == null)
                {
                    _logger.LogWarning(ResponseMessage.unauthorizedAttempt);

                    return Unauthorized(new { success = false, message = ResponseMessage.unauthorizeUser });
                }



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





        /// <summary>
        /// API to insert question created by authenticated user
        /// </summary>
        /// <param name="questionDetails"></param>
        /// <returns>Success message</returns>

        [HttpPost("AddQuestion")]
        [Authorize]
        public async Task<IActionResult> AddQuestion(QuestionDto questionDetails)
        {
            try
            {
                var authHeader = Request.Headers["Authorization"].FirstOrDefault();

                if (authHeader == null || !authHeader.StartsWith("Bearer "))
                {
                    _logger.LogWarning(ResponseMessage.unauthorizedAttempt);

                    return Unauthorized(new { success = false, message = ResponseMessage.unauthorizeUser });
                }

                var token = authHeader.Substring("Bearer ".Length).Trim();

                //Validate token condtion if we have to use jwt
                var claims = await _jwtService.ValidateJwtToken(token);

                if (claims == null)
                {
                    _logger.LogWarning(ResponseMessage.unauthorizedAttempt);

                    return Unauthorized(new { success = false, message = ResponseMessage.unauthorizeUser });
                }

                var userId = int.Parse(claims["Id"]);



                var res = await _questionService.AddQuestion(questionDetails, userId);

                if (res.success)
                {
                    return Ok(new { success = res.success, message = ResponseMessage.questionCreationSuccess, questionId = res.questionId });
                }
                else
                {
                    return StatusCode(500, new { success = res.success, message = ResponseMessage.internalServerError });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { success = false, message = ResponseMessage.internalServerError, error = ex.Message });
            }
        }





        /// <summary>
        /// API to get all question created by user using userId(extracted from jwt token)
        /// </summary>
        /// <returns>List of questions</returns>

        [HttpGet("GetAllQuestions")]
        [Authorize]
        public async Task<IActionResult> GetAllQuestions()
        {
            try
            {
                var authHeader = Request.Headers["Authorization"].FirstOrDefault();

                if (authHeader == null || !authHeader.StartsWith("Bearer "))
                {
                    _logger.LogWarning(ResponseMessage.unauthorizedAttempt);

                    return Unauthorized(new { success = false, message = ResponseMessage.unauthorizeUser });
                }

                var token = authHeader.Substring("Bearer ".Length).Trim();

                //Validate token condtion if we have to use jwt
                var claims = await _jwtService.ValidateJwtToken(token);

                if (claims == null)
                {
                    _logger.LogWarning(ResponseMessage.unauthorizedAttempt);

                    return Unauthorized(new { success = false, message = ResponseMessage.unauthorizeUser });
                }
                var userId = int.Parse(claims["Id"]);


                var questions = await _questionService.GetAllQuestions(userId);

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




        /// <summary>
        /// Get single question with all its options if present using userId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Question details</returns>

        [HttpGet("GetQuestionById/{id}")]
        [Authorize]
        public async Task<IActionResult> GetQuestionById(int id)
        {
            try
            {
                var authHeader = Request.Headers["Authorization"].FirstOrDefault();

                if (authHeader == null || !authHeader.StartsWith("Bearer "))
                {
                    _logger.LogWarning(ResponseMessage.unauthorizedAttempt);

                    return Unauthorized(new { success = false, message = ResponseMessage.unauthorizeUser });
                }

                var token = authHeader.Substring("Bearer ".Length).Trim();

                //Validate token condtion if we have to use jwt
                var claims = await _jwtService.ValidateJwtToken(token);

                if (claims == null)
                {
                    _logger.LogWarning(ResponseMessage.unauthorizedAttempt);

                    return Unauthorized(new { success = false, message = ResponseMessage.unauthorizeUser });
                }

                var userId = int.Parse(claims["Id"]);



                var question = await _questionService.GetQuestion(id, userId);

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





        /// <summary>
        /// API to remove the question using id and userId(extracted from jwt token)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>success message</returns>

        [HttpDelete("RemoveQuestionById/{id}")]
        [Authorize]
        public async Task<IActionResult> RemoveQuestionById(int id)
        {
            try
            {
                var authHeader = Request.Headers["Authorization"].FirstOrDefault();

                if (authHeader == null || !authHeader.StartsWith("Bearer "))
                {
                    _logger.LogWarning(ResponseMessage.unauthorizedAttempt);

                    return Unauthorized(new { success = false, message = ResponseMessage.unauthorizeUser });
                }

                var token = authHeader.Substring("Bearer ".Length).Trim();

                //Validate token condtion if we have to use jwt
                var claims = await _jwtService.ValidateJwtToken(token);

                if (claims == null)
                {
                    _logger.LogWarning(ResponseMessage.unauthorizedAttempt);

                    return Unauthorized(new { success = false, message = ResponseMessage.unauthorizeUser });
                }

                var userId = int.Parse(claims["Id"]);




                var res = await _questionService.DeleteQuestionById(id, userId);

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





        /// <summary>
        /// API to update question using questionId and userId(extracted from jwt token)
        /// </summary>
        /// <param name="questionDetails"></param>
        /// <returns>Success message</returns>

        [HttpPut("UpdateQuestion")]
        [Authorize]
        public async Task<IActionResult> UpdateQuestion([FromBody] QuestionDto questionDetails)
        {
            try
            {
                var authHeader = Request.Headers["Authorization"].FirstOrDefault();

                if (authHeader == null || !authHeader.StartsWith("Bearer "))
                {
                    _logger.LogWarning(ResponseMessage.unauthorizedAttempt);

                    return Unauthorized(new { success = false, message = ResponseMessage.unauthorizeUser });
                }

                var token = authHeader.Substring("Bearer ".Length).Trim();

                //Validate token condtion if we have to use jwt
                var claims = await _jwtService.ValidateJwtToken(token);

                if (claims == null)
                {
                    _logger.LogWarning(ResponseMessage.unauthorizedAttempt);

                    return Unauthorized(new { success = false, message = ResponseMessage.unauthorizeUser });
                }


                var userId = int.Parse(claims["Id"]);





                var result = await _questionService.UpdateQuestion(questionDetails, userId);

                if (result.success)
                {
                    return Ok(new { success = result.success, message = ResponseMessage.questionUpdateSuccess });
                }
                else
                {
                    return StatusCode(500, new { success = result.success, message = ResponseMessage.internalServerError });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { success = false, message = ResponseMessage.internalServerError, error = ex.Message });
            }
        }

       



    }
}
