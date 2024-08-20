using DynamicFormsBackend.Models.Dto;
using DynamicFormsBackend.Models.Entities;
using DynamicFormsBackend.Service.Authentication;
using DynamicFormsBackend.ServiceInterface.Authentication;
using DynamicFormsBackend.ServiceInterface.FormCreation;
using DynamicFormsBackend.Shared;
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

        public FormTemplateController(ILogger<FormTemplateController> logger, IFormService formService)
        {
            _logger = logger;
            _logger.LogDebug("Nlog is integrated to Form Template controller");
            
            _formService = formService;

        }


        [HttpPost("CreateTemplate")]
        public async Task<IActionResult> CreateTemplate([FromBody] SourceTemplateDto templateDto)
        {
            if (templateDto == null || templateDto.Sections == null || !templateDto.Sections.Any())
            {
                return BadRequest(new {success=false, message= ResponseMessage.NullTemplateError });
            }

            try
            {
                var res = await _formService.AddSourceTemplate(templateDto);
                

                //For publish
                if (res.success && templateDto.IsPublish == true)
                {
                    var formLink = $"https://localhost:4200/forms/form/?formId={res.formid}";

                    return Ok(new { success = res.success, link=formLink, message = ResponseMessage.sourceTemplateCreationSuccess });
                }

                //For draft save
                else if(res.success && templateDto.IsPublish == false) 
                {
                    return Ok(new { succes = res, message = ResponseMessage.sourceTemplateCreationSuccess });
                }

                return StatusCode(500, new { success = false, message = ResponseMessage.internalServerError });
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { success = false, message = ResponseMessage.internalServerError, error = ex.Message });
            }
        }


        [HttpGet("GetAllForms")]
        public async Task<IActionResult> GetAllForms()
        {
            try
            {
                var res = await _formService.Getforms();

                if (res != null)
                {
                    return Ok(new { succes = true, forms=res });
                }

                return StatusCode(500, new { success = false, message = ResponseMessage.internalServerError });
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { success = false, message = ResponseMessage.internalServerError, error = ex.Message });
            }
        }


        [HttpGet("GetFormById")]
        public async Task<IActionResult> GetFormById(int formId)
        {
            try
            {
                var res = await _formService.GetFormById(formId);

                if(res != null && res.IsPublish == true)
                {
                    var formLink = $"https://localhost:4200/forms/form/?formId={res.Id}";

                    return Ok(new { success = true, form = res , link=formLink });
                    
                }

                else if (res != null && res.IsPublish == false)
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



        [HttpDelete("RemoveFormById/{id}")]
        public async Task<IActionResult> RemoveFormById(int formId)
        {
            try
            {
                var res = await _formService.RemoveFormById(formId);

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

        


    }
}
