using Microsoft.AspNetCore.Mvc;
using StudentRecruitment.BLL.Services;
using StudentRecruitment.Shared.DTOs;

namespace StudentRecruitment.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : Controller
    {
        private readonly StudentService _studentService;

        public StudentController(StudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost("get-students-rating")]
        public async Task<IActionResult> GetStudentsRating([FromBody] List<SubjectRatingDto> subjectRatingDtos, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 30)
        {
            var result = await _studentService.GetBestSuitedStudentsAsync(subjectRatingDtos, pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("{studentId}/grades")]
        public async Task<IActionResult> GetStudentWithGrades(int studentId)
        {
            var studentWithGrades = await _studentService.GetStudentWithGradesAsync(studentId);
            if (studentWithGrades == null)
            {
                return NotFound();
            }
            return Ok(studentWithGrades);
        }
    }
}