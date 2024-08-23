using DynamicFormsBackend.Models.Dto;
using DynamicFormsBackend.ServiceInterface.FormCreation;
using DynamicFormsBackend.ServiceInterface.Response;
using DynamicFormsBackend.Shared;
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

        public FormResponseController(ILogger<FormResponseController> logger, IFormResponseService formResponseService)
        {
            _logger = logger;
            _logger.LogDebug("Nlog is integrated to Form Template controller");

            _formResponseService = formResponseService;
        }


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



        [HttpGet("GetAllResponseByFormId/{formId}")]
        public async Task<IActionResult> GetAllResponses(int formId)
        {
            try
            {
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



        [HttpGet("GetResponseById/{id}")]
        public async Task<IActionResult> GetResponseById(int id)
        {
            try
            {
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


        [HttpDelete("DeleteResponse/{id}")]
        public async Task<IActionResult> DeleteResponse(int id)
        {
            try
            {
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
