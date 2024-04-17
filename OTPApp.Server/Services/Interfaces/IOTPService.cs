using OTPApp.Server.Models;

namespace OTPApp.Server.Services.Interfaces
{
    public interface IOTPService
    {
        OTPToken GenerateOtpToken();

        bool ValidateToken(OTPToken otpToken);
    }
}
