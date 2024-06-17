using Microsoft.AspNetCore.Mvc;
using StudentRecruitment.BLL.Services;
using StudentRecruitment.Shared.DTOs;

namespace StudentRecruitment.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SystemManagementController : Controller
    {
        private readonly SubjectService _subjectService;
        private readonly StudentService _studentService;
        private readonly EmployerService _employerService;

        public SystemManagementController(SubjectService subjectService, StudentService studentService, EmployerService employerService)
        {
            _subjectService = subjectService;
            _studentService = studentService;
            _employerService = employerService;
        }

        [HttpPost("import-subjects-data")]
        public async Task<IActionResult> ImportSubjects()
        {
            await _subjectService.ImportSubjectsFromFileAsync();
            return Ok(new { Message = "Subjects have been successfully imported."});
        }

        [HttpPost("import-students-data")]
        public async Task<IActionResult> ImportStudents()
        {
            await _studentService.ImportStudentsFromFileAsync();
            return Ok(new { Message = "Students have been successfully imported and accounts created." });
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto request)
        {
            bool result;
            if (request.UserType == "student")
            {
                result = await _studentService.ChangePasswordAsync(request.Username, request.NewPassword);
            }
            else if (request.UserType == "employer")
            {
                result = await _employerService.ChangePasswordAsync(request.Username, request.NewPassword);
            }
            else
            {
                return BadRequest(new { Message = "Invalid user type." });
            }

            if (result)
            {
                return Ok(new { Message = "Password has been successfully changed." });
            }
            else
            {
                return BadRequest(new { Message = "Failed to change password or user not found." });
            }
        }

        [HttpDelete("delete-all-students")]
        public async Task<IActionResult> DeleteAllStudents()
        {
            var result = await _studentService.DeleteAllStudentsAsync();
            if (result)
            {
                return Ok(new { Message = "All students have been successfully deleted." });
            }
            else
            {
                return BadRequest(new { Message = "Failed to delete students or no students found." });
            }
        }

        [HttpDelete("delete-all-employers")]
        public async Task<IActionResult> DeleteAllEmployers()
        {
            var result = await _employerService.DeleteAllEmployersAsync();
            if (result)
            {
                return Ok(new { Message = "All employers have been successfully deleted." });
            }
            else
            {
                return BadRequest(new { Message = "Failed to delete employers or no employers found." });
            }
        }


        [HttpDelete("delete-all-subjects")]
        public async Task<IActionResult> DeleteAllSubjects()
        {
            var result = await _subjectService.DeleteAllSubjectsAsync();
            if (result)
            {
                return Ok(new { Message = "All subjects have been successfully deleted." });
            }
            else
            {
                return BadRequest(new { Message = "Failed to delete subjects or no subjects found." });
            }
        }
    }
}