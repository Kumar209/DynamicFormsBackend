using DynamicFormsBackend.Models.Dto;
using DynamicFormsBackend.ServiceInterface.Authentication;
using DynamicFormsBackend.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DynamicFormsBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            _logger = logger;
            _logger.LogDebug("Nlog is integrated to Auth controller");

            _authService = authService;
        }


        [HttpPost("LoginUser")]
        public async Task<IActionResult> login(LoginCredential credential)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError(ResponseMessage.validationFailed);
                return BadRequest(new { success = false, message = ResponseMessage.validationFailed , model=ModelState});
            }


            try
            {
                var res = await _authService.AuthenticateUser(credential.Email, credential.Password);

                if (res.success)
                {
                    return Ok(new {success = res.success , message = ResponseMessage.loginSuccess , token=res.token });
                }

                return BadRequest(new { success = res.success, message = ResponseMessage.wrongCredential });
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { success = false, message = ResponseMessage.internalServerError, error = ex.Message });
            }


        }
    }
}
