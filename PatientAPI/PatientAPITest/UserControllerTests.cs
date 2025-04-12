using Microsoft.AspNetCore.Mvc;
using Moq;
using PatientAPI.Controllers;
using PatientSystem.Services.Interfaces;
using Xunit;

namespace PatientManagementAPI.Tests
{
    public class UserControllerTests
    {
        private readonly Mock<IJwtService> _mockJwtService;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mockJwtService = new Mock<IJwtService>();
            _controller = new UserController(_mockJwtService.Object);
        }

        public class LoginTestData
        {
            public static IEnumerable<object[]> ValidLoginData =>
                new List<object[]>
                {
                    new object[] { "admin", "admin-token", new List<string> { "reader", "writer", "admin" } },
                    new object[] { "viewer", "viewer-token", new List<string> { "reader" } },
                    new object[] { "unauthorized", "unauthorized-token", new List<string>() }
                };
        }

        [Theory]
        [MemberData(nameof(LoginTestData.ValidLoginData), MemberType = typeof(LoginTestData))]
        public void Login_WithValidCredentials_ReturnsExpectedToken(string username, string expectedToken, List<string> expectedRoles)
        {
            var loginRequest = new LoginRequest { Username = username };
            _mockJwtService
                .Setup(x => x.GenerateToken(username, It.IsAny<List<string>>(), It.IsAny<int>()))
                .Returns(expectedToken);

            List<string> setupRoles = new List<string>();
            if (username == "admin")
            {
                _mockJwtService
                    .Setup(x => x.GenerateToken(username, 
                    new List<string> { "reader", "writer", "admin" }, It.IsAny<int>()))
                    .Returns(expectedToken);
            }
            else if (username == "viewer")
            {
                _mockJwtService
                    .Setup(x => x.GenerateToken(username,
                    new List<string> { "writer", "admin" }, It.IsAny<int>()))
                    .Returns(expectedToken);
            }
            else
            {
                _mockJwtService
                    .Setup(x => x.GenerateToken(username,
                    new List<string> {  }, It.IsAny<int>()))
                    .Returns(expectedToken);
            }

            var result = _controller.Login(loginRequest);
            
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            var resultObj = okResult.Value;
            Assert.NotNull(resultObj);
            
            var tokenProp = resultObj.GetType().GetProperty("token");
            Assert.NotNull(tokenProp);
            Assert.Equal(expectedToken, tokenProp.GetValue(resultObj));
            
            var usernameProp = resultObj.GetType().GetProperty("username");
            Assert.NotNull(usernameProp);
            Assert.Equal(username, usernameProp.GetValue(resultObj));
            
            var rolesProp = resultObj.GetType().GetProperty("roles");
            Assert.NotNull(rolesProp);
            var roles = rolesProp.GetValue(resultObj) as List<string>;
            
            if (expectedRoles.Count > 0)
            {
                foreach (var role in expectedRoles)
                {
                    Assert.Contains(role, roles);
                }
                
                if (!expectedRoles.Contains("writer"))
                {
                    Assert.DoesNotContain("writer", roles);
                }
                
                if (!expectedRoles.Contains("admin"))
                {
                    Assert.DoesNotContain("admin", roles);
                }
            }
            else
            {
                Assert.Empty(roles);
            }
        }

        [Fact]
        public void Login_WithInvalidCredentials_ReturnsUnauthorized()
        {
            var loginRequest = new LoginRequest { Username = "invalid" };

            var result = _controller.Login(loginRequest) as UnauthorizedObjectResult;

            Assert.NotNull(result);
            Assert.Equal(401, result.StatusCode);
        }
    }
}
