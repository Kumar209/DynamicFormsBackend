using DynamicFormsBackend.Models.Dto;
using DynamicFormsBackend.ServiceInterface;
using DynamicFormsBackend.ServiceInterface.FormCreation;
using DynamicFormsBackend.ServiceInterface.Response;
using DynamicFormsBackend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DynamicFormsBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormResponseController : ControllerBase
    {
        private readonly ILogger<FormResponseController> _logger;
        private readonly IFormResponseService _formResponseService;
        private readonly IJwtService _jwtService;

        public FormResponseController(ILogger<FormResponseController> logger, IFormResponseService formResponseService, IJwtService jwtService)
        {
            _logger = logger;
            _logger.LogDebug("Nlog is integrated to Form Template controller");

            _formResponseService = formResponseService;
            _jwtService = jwtService;
        }


        /// <summary>
        /// API for inserting the response filled by the audience of user
        /// </summary>
        /// <returns>Success message</returns>

        [HttpPost("InsertResponse")]
        public async Task<IActionResult> InsertResponse(FormResponseDto response)
        {
            try
            {
                var res = await _formResponseService.AddFormResponse(response);

                if (res == true)
                {
                    return Ok(new { success = res, message = ResponseMessage.resonseInsertedSuccess });
                }

                return StatusCode(500, new { success = res, message = ResponseMessage.internalServerError });
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { success = false, message = ResponseMessage.internalServerError, error = ex.Message });
            }
        }




        /// <summary>
        /// API for getting all responses related to single form created by user
        /// </summary>
        /// <param name="formId"></param>
        /// <returns>List of response</returns>

        [HttpGet("GetAllResponseByFormId/{formId}")]
        [Authorize]
        public async Task<IActionResult> GetAllResponses(int formId)
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



                var res = await _formResponseService.GetAllResponse(formId);

                if (res != null)
                {
                    return Ok(new { success = true, responses = res });
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
        /// API for getting single response of filled by audience
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Response</returns>

        [HttpGet("GetResponseById/{id}")]
        [Authorize]
        public async Task<IActionResult> GetResponseById(int id)
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



                var res = await _formResponseService.GetResponse(id);

                if (res != null)
                {
                    return Ok(new { success = true, responses = res });
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
        /// API to delete the response of any form created by user
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Success message</returns>

        [HttpDelete("DeleteResponse/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteResponse(int id)
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




                var res = await _formResponseService.softDeleteResponse(id);

                if (res != null)
                {
                    return Ok(new { success = res, responses = ResponseMessage.SuccessResponseDeletion });
                }

                return StatusCode(500, new { success = false, message = ResponseMessage.internalServerError });
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { success = false, message = ResponseMessage.internalServerError, error = ex.Message });
            }
        }

    }
}
