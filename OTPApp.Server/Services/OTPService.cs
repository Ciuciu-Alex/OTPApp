using OTPApp.Server.Models;
using OTPApp.Server.Services.Interfaces;
using System.Security.Cryptography;

namespace OTPApp.Server.Services
{
    public class OTPService : IOTPService
    {
        private const int OTP_LENGTH = 12;
        private const int EXPIRY_MINUTES = 5;

        public OTPToken GenerateOtpToken()
        {
            return new OTPToken
            {
                Token = GenerateRandomOTP(),
                ExpiryTime = DateTime.UtcNow.AddMinutes(EXPIRY_MINUTES)
            };
        }

        public bool ValidateToken(OTPToken otpToken)
        {
            return otpToken.ExpiryTime < DateTime.UtcNow;
        }

        private string GenerateRandomOTP()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var bytes = new byte[OTP_LENGTH];
                rng.GetBytes(bytes);
                return BitConverter.ToString(bytes).Replace("-", "").Substring(0, OTP_LENGTH);
            }
        }
    }
}
