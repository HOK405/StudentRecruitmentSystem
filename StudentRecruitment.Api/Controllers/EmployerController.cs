using Microsoft.AspNetCore.Mvc;
using StudentRecruitment.BLL.Services;
using StudentRecruitment.Shared.DTOs;

namespace StudentRecruitment.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployerController : Controller
    {
        private readonly EmployerService _employerService;

        public EmployerController(EmployerService employerService)
        {
            _employerService = employerService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateEmployer([FromBody] CreateEmployerDto createEmployerDto)
        {
            try
            {
                var employer = await _employerService.CreateEmployerAsync(createEmployerDto);
                return Ok(employer);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}
