using Microsoft.AspNetCore.Mvc;
using OTPApp.Server.Models;
using OTPApp.Server.Services.Interfaces;

namespace OTPApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OtpController : Controller
    {
        private readonly IOTPService _otpService;

        public OtpController(IOTPService otpService)
        {
            _otpService = otpService;
        }

        [HttpGet]
        public IActionResult GenerateOTP()
        {
            OTPToken otpValidationRequest = _otpService.GenerateOtpToken();

            return Ok(otpValidationRequest);
        }

        [HttpPost]
        [Route("validate")]
        public IActionResult ValidateOTP([FromBody] OTPToken request)
        {
            if (_otpService.ValidateToken(request))
            {
                return BadRequest("OTP expired.");
            }

            return Ok(request);
        }
    }
}
