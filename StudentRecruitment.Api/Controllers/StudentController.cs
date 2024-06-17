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
        public async Task<IActionResult> GetStudentsRating(
            [FromBody] List<SubjectRatingDto> subjectRatingDtos, 
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 30)
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

        [HttpPut("{studentId}/description")]
        public async Task<IActionResult> UpdateStudentDescription(int studentId, [FromBody] string newDescription)
        {
            var result = await _studentService.UpdateStudentDescriptionAsync(studentId, newDescription);
            if (!result)
            {
                return NotFound();
            }

            return Ok(new { Message = "Description has been successfully updated." });
        }

        [HttpPut("{studentId}/is-public-profile")]
        public async Task<IActionResult> UpdateIsPublicProfile(int studentId, [FromBody] bool isPublicProfile)
        {
            var result = await _studentService.UpdateIsPublicProfileAsync(studentId, isPublicProfile);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }


        [HttpDelete("{studentId}")]
        public async Task<IActionResult> DeleteStudent(int studentId)
        {
            var result = await _studentService.DeleteStudentByIdAsync(studentId);
            if (!result)
            {
                return NotFound();
            }

            return Ok(new { Message = "Student has been successfully deleted." });
        }
    }
}