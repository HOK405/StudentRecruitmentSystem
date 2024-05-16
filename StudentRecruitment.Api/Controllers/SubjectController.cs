using Microsoft.AspNetCore.Mvc;
using StudentRecruitment.BLL.Services;

namespace StudentRecruitment.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectController : Controller
    {
        private readonly SubjectService _subjectService;

        public SubjectController(SubjectService subjectService)
        {
            _subjectService = subjectService;
        }
        [HttpGet("get-subjects")]
        public async Task<IActionResult> GetSubjects([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 30)
        {
            var pagedSubjects = await _subjectService.GetPagedSubjectsAsync(pageNumber, pageSize);
            return Ok(pagedSubjects);
        }
    }
}
