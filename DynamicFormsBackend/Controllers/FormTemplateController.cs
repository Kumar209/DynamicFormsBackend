using DynamicFormsBackend.Models.Dto;
using DynamicFormsBackend.Models.Entities;
using DynamicFormsBackend.Service.Authentication;
using DynamicFormsBackend.ServiceInterface;
using DynamicFormsBackend.ServiceInterface.Authentication;
using DynamicFormsBackend.ServiceInterface.FormCreation;
using DynamicFormsBackend.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;

namespace DynamicFormsBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormTemplateController : ControllerBase
    {
        private readonly ILogger<FormTemplateController> _logger;
        private readonly IFormService _formService;
        private readonly IJwtService _jwtService;

        public FormTemplateController(ILogger<FormTemplateController> logger, IFormService formService, IJwtService jwtService)
        {
            _logger = logger;
            _logger.LogDebug("Nlog is integrated to Form Template controller");
            
            _formService = formService;
            _jwtService = jwtService;

        }




        /// <summary>
        /// API to created form template with sections and section-question mapping by user
        /// </summary>
        /// <param name="templateDto"></param>
        /// <returns>Success message</returns>

        [HttpPost("CreateTemplate")]
        [Authorize]
        public async Task<IActionResult> CreateTemplate([FromBody] SourceTemplateDto templateDto)
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

            if(claims == null)
            {
                _logger.LogWarning(ResponseMessage.unauthorizedAttempt);

                return Unauthorized(new { success = false, message = ResponseMessage.unauthorizeUser });
            }


            if (templateDto == null || templateDto.Sections == null || !templateDto.Sections.Any())
            {
                return BadRequest(new {success=false, message= ResponseMessage.NullTemplateError });
            }

            // Validate that at least one question ID is present in each section
            foreach (var section in templateDto.Sections)
            {
                if (section.SelectedQuestions == null || !section.SelectedQuestions.Any())
                {
                    return BadRequest(new { success = false, message = ResponseMessage.NoQuestionsInSectionError });
                }
            }

            try
            {
                var userId = int.Parse(claims["Id"]);
                var res = await _formService.AddSourceTemplate(templateDto, userId);
                

                //For publish
                if (res.success && templateDto.IsPublish == true)
                {
                    var formLink = $"http://localhost:4200/form-response/generated-form?formId={res.formid}";

                    return Ok(new { success = res.success, link=formLink, message = ResponseMessage.sourceTemplateCreationSuccess });
                }

                //For draft save
                else if(res.success && templateDto.IsPublish == false) 
                {
                    return Ok(new { success = res, message = ResponseMessage.sourceTemplateCreationSuccess });
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
        /// API to get all forms created by user using userId extracted from jwt token
        /// </summary>
        /// <returns>List of forms related to user</returns>

        [HttpGet("GetAllForms")]
        [Authorize]
        public async Task<IActionResult> GetAllForms()
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

                var res = await _formService.Getforms(userId);

                if (res != null)
                {
                    return Ok(new { success = true, forms=res });
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
        /// API to get single form all details using formId and userId(extract from jwt token)
        /// </summary>
        /// <param name="formId"></param>
        /// <returns>Single form with sections and questions</returns>

        [HttpGet("GetFormById/{formId}")]
        [Authorize]
        public async Task<IActionResult> GetFormById(int formId)
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

                var res = await _formService.GetFormById(formId, userId);

                if(res != null)
                {
                    return Ok(new { success = true, form = res });
                    
                }


                return StatusCode(500, new { success = false, message = ResponseMessage.NotFoundForm });




            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { success = false, message = ResponseMessage.internalServerError, error = ex.Message });
            }
        }




        /// <summary>
        /// API to soft delete the form created by user using formId and userId(extract from jwt token)
        /// </summary>
        /// <param name="formId"></param>
        /// <returns>Success message</returns>

        [HttpDelete("RemoveFormById/{formId}")]
        [Authorize]
        public async Task<IActionResult> RemoveFormById(int formId)
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

                var res = await _formService.RemoveFormById(formId, userId);

                if (res)
                {
                    return Ok(new { succes = res, forms = ResponseMessage.formDeleted });
                }

                return StatusCode(500, new { success = false, message = ResponseMessage.NotFoundForm});
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { success = false, message = ResponseMessage.internalServerError, error = ex.Message });
            }
        }




        /// <summary>
        /// API to update form created by user using formId and userId(extract from jwt token)
        /// </summary>
        /// <param name="formId"></param>
        /// <returns>Success Message</returns>

        [HttpPut("UpdateTemplate/{formId}")]
        [Authorize]
        public async Task<IActionResult> UpdateTemplate(int formId, [FromBody] SourceTemplateDto templateDto)
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



            if (templateDto == null || templateDto.Sections == null || !templateDto.Sections.Any())
            {
                return BadRequest(new { success = false, message = ResponseMessage.NullTemplateError });
            }

            // Validate that at least one question ID is present in each section
            foreach (var section in templateDto.Sections)
            {
                if (section.SelectedQuestions == null || !section.SelectedQuestions.Any())
                {
                    return BadRequest(new { success = false, message = ResponseMessage.NoQuestionsInSectionError });
                }
            }

            try
            {
                var userId = int.Parse(claims["Id"]);

                var res = await _formService.UpdateSourceTemplate(formId, templateDto, userId);

                if (res)
                {
                    return Ok(new { success = res, message = ResponseMessage.sourceTemplateUpdateSuccess });
                }

                return StatusCode(500, new { success = res, message = ResponseMessage.internalServerError });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { success = false, message = ResponseMessage.internalServerError, error = ex.Message });
            }
        }




    }
}
