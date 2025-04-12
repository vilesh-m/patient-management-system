using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PatientSystem.Services;
using PatientSystem.Services.Interfaces;
using Xunit;

namespace PatientManagementAPI.Tests
{
    public class JwtServiceTests
    {
        private readonly IJwtService _jwtService;
        private readonly byte[] _jwtKey;

        public JwtServiceTests()
        {
            _jwtKey = Encoding.ASCII.GetBytes("ThisIsASecretKeyForTestingPurposesOnly12345");
            _jwtService = new JwtService();
        }

        [Fact]
        public void GenerateToken_WithValidData_ReturnsValidToken()
        {
            string username = "testuser";
            var roles = new List<string> { "reader", "writer" };
            int expiryMinutes = 5;

            string token = _jwtService.GenerateToken(username, roles, expiryMinutes);

            Assert.NotNull(token);
            Assert.NotEmpty(token);
            
            var tokenHandler = new JwtSecurityTokenHandler();
            Assert.True(tokenHandler.CanReadToken(token));
            var jwtToken = tokenHandler.ReadJwtToken(token);

            var claims = jwtToken.Claims.ToList();
            Assert.NotEmpty(claims);

            Assert.Equal(username, jwtToken.Claims.FirstOrDefault(c => c.Type == "unique_name")?.Value);
            var actualRoles = jwtToken.Claims.Where(c => c.Type == "role").Select(x=>x.Value);
            Assert.Equal(roles, actualRoles);

            var expectedExpiry = DateTime.UtcNow.AddMinutes(expiryMinutes);
            var tokenExpiry = jwtToken.ValidTo;
            Assert.True(Math.Abs((expectedExpiry - tokenExpiry).TotalSeconds) < 5);
        }
    }
}
