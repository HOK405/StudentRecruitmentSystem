using Microsoft.AspNetCore.Identity;
using StudentRecruitment.Domain.Entities;
using StudentRecruitment.Shared.DTOs;

namespace StudentRecruitment.DAL.Interfaces
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetBestSuitedStudentsAsync(Dictionary<int, int> subjectRatings);

        Task<List<CredentialDto>> ImportStudentsAsync(
            List<StudentImportDto> students,
            UserManager<Student> userManager);
    }
}