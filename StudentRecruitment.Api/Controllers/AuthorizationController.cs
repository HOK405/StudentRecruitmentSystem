using Microsoft.AspNetCore.Mvc;
using StudentRecruitment.BLL.Services;
using StudentRecruitment.Shared.DTOs;

namespace StudentRecruitment.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizationController : Controller
    {
        private readonly JwtTokenService _jwtTokenService;

        public AuthorizationController(JwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost]
        public async Task<IActionResult> GetToken([FromBody] CredentialDto loginRequest)
        {
            try
            {
                var token = await _jwtTokenService.GenerateAccessTokenAsync(loginRequest.Username, loginRequest.Password);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}
