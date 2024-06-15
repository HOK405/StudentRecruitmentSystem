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

        [HttpPost("like")]
        public async Task<IActionResult> LikeStudent([FromBody] LikeStudentDto likeStudentDto)
        {
            try
            {
                await _employerService.LikeStudentAsync(likeStudentDto.EmployerId, likeStudentDto.StudentId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpGet("liked-students/{employerId}")]
        public async Task<IActionResult> GetLikedStudents(int employerId)
        {
            try
            {
                var likedStudents = await _employerService.GetLikedStudentsAsync(employerId);
                return Ok(likedStudents);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpDelete("dislike-student/{employerId}/{studentId}")]
        public async Task<IActionResult> DislikeStudent(int employerId, int studentId)
        {
            try
            {
                await _employerService.DislikeStudentAsync(employerId, studentId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}
