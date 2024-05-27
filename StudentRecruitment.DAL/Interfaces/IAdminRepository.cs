using StudentRecruitment.Domain.Entities;

namespace StudentRecruitment.DAL.Interfaces
{
    public interface IAdminRepository
    {
        Task<Admin> GetAdminByIdAsync(int adminId);

        Task<Admin> CreateAdminAsync(Admin admin);
    }
}
