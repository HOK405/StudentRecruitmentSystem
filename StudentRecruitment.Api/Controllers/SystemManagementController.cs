using Microsoft.AspNetCore.Mvc;
using StudentRecruitment.BLL.Services;

namespace StudentRecruitment.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SystemManagementController : Controller
    {
        private readonly SubjectService _subjectService;
        private readonly StudentService _studentService;

        public SystemManagementController(SubjectService subjectService, StudentService studentService)
        {
            _subjectService = subjectService;
            _studentService = studentService;
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
    }
}