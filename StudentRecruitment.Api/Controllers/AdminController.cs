using Microsoft.AspNetCore.Mvc;
using StudentRecruitment.BLL.Services;
using StudentRecruitment.Shared.DTOs;

namespace StudentRecruitment.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly AdminService _adminService;

        public AdminController(AdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAdmin([FromBody] CreateAdminDto createAdminDto)
        {
            try
            {
                var admin = await _adminService.CreateAdminAsync(createAdminDto);
                return Ok(admin);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}
