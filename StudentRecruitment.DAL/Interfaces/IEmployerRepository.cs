using StudentRecruitment.Domain.Entities;

namespace StudentRecruitment.DAL.Interfaces
{
    public interface IEmployerRepository
    {
        Task<Employer> GetEmployerByIdAsync(int employerId);
    }
}
