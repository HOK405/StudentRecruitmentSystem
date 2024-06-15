using StudentRecruitment.Domain.Entities;

namespace StudentRecruitment.DAL.Interfaces
{
    public interface IEmployerRepository
    {
        Task<Employer> GetEmployerByIdAsync(int employerId);

        Task LikeStudentAsync(int employerId, int studentId);

        Task<List<Student>> GetLikedStudentsAsync(int employerId);

        Task DislikeStudentAsync(int employerId, int studentId);
    }
}