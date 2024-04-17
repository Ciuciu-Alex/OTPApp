namespace OTPApp.Server.Models
{
    public class OTPToken
    {
        public string Token { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
}
