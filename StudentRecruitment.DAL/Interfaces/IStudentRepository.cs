using Microsoft.AspNetCore.Identity;
using StudentRecruitment.Domain.Entities;
using StudentRecruitment.Shared.DTOs;

namespace StudentRecruitment.DAL.Interfaces
{
    public interface IStudentRepository
    {
        Task<Student> GetStudentByIdAsync(int studentId);

        Task<List<Student>> GetBestSuitedStudentsAsync(Dictionary<int, int> subjectRatings);

        Task<List<CredentialDto>> ImportStudentsAsync(
            List<StudentImportDto> students,
            UserManager<IdentityUser<int>> userManager);

        Task<Student> GetStudentWithGradesAsync(int studentId);

        Task DeleteStudentAsync(Student student);

        Task SaveChangesAsync();
    }
}