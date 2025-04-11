using Microsoft.AspNetCore.Mvc;
using PatientManagementAPI.Services;
using System.Collections.Generic;
using System.Text.Json;

namespace PatientAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class UserController : ControllerBase
    {
        private readonly JwtService _jwtService;

        public UserController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null)
            {
                return BadRequest(new { message = "Invalid request format" });
            }

            string username = loginRequest.Username;

            if (username == "admin")
            {
                var roles = new List<string> { "reader", "writer", "admin" };
                var token = _jwtService.GenerateToken(username, roles);
                
                return Ok(new { 
                    token,
                    username,
                    roles
                });
            }
            else if (username == "viewer")
            {
                var roles = new List<string> { "reader" };
                var token = _jwtService.GenerateToken(username, roles);
                
                return Ok(new { 
                    token,
                    username,
                    roles
                });
            }
            else if (username == "unauthorized")
            {
                var roles = new List<string>();
                var token = _jwtService.GenerateToken(username, roles);
                
                return Ok(new { 
                    token,
                    username,
                    roles
                });
            }

            return Unauthorized(new { message = "Invalid credentials" });
        }
    }
}
