using Microsoft.AspNetCore.Mvc;
using Moq;
using OTPApp.Server.Controllers;
using OTPApp.Server.Models;
using OTPApp.Server.Services.Interfaces;

namespace OTPTests
{
    [TestClass]
    public class OTPTest
    {
        [TestClass]
        public class OtpControllerTests
        {
            private OtpController _controller;
            private Mock<IOTPService> _otpServiceMock;

            [TestInitialize]
            public void Setup()
            {
                // Initialize the mock of IOTPService
                _otpServiceMock = new Mock<IOTPService>();
                _controller = new OtpController(_otpServiceMock.Object);
            }

            [TestMethod]
            public void GenerateOTP_ReturnsOk()
            {
                // Arrange
                var expectedOTPToken = new OTPToken(); // Create your expected OTPToken object

                _otpServiceMock.Setup(s => s.GenerateOtpToken()).Returns(expectedOTPToken);

                // Act
                var result = _controller.GenerateOTP() as OkObjectResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(200, result.StatusCode);
                Assert.AreEqual(expectedOTPToken, result.Value); // Assuming OTPToken implements Equals method
            }

            [TestMethod]
            public void ValidateOTP_WithValidToken_ReturnsOk()
            {
                // Arrange
                var request = new OTPToken(); // Create your OTPToken object
                _otpServiceMock.Setup(s => s.ValidateToken(request)).Returns(false);

                // Act
                var result = _controller.ValidateOTP(request) as OkObjectResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(200, result.StatusCode);
                Assert.AreEqual(request, result.Value); // Assuming OTPToken implements Equals method
            }

            [TestMethod]
            public void ValidateOTP_WithExpiredToken_ReturnsBadRequest()
            {
                // Arrange
                var request = new OTPToken(); // Create your OTPToken object
                _otpServiceMock.Setup(s => s.ValidateToken(request)).Returns(true);

                // Act
                var result = _controller.ValidateOTP(request) as BadRequestObjectResult;

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(400, result.StatusCode);
                Assert.AreEqual("OTP expired.", result.Value);
            }
        }
    }
}