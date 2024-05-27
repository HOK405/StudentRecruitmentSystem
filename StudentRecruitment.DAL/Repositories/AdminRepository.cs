using StudentRecruitment.DAL.Interfaces;
using StudentRecruitment.Domain.Entities;

namespace StudentRecruitment.DAL.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApidDbContext _dbContext;

        public AdminRepository(ApidDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Admin> GetAdminByIdAsync(int adminId)
        {
            return await _dbContext.Admins.FindAsync(adminId);
        }

        public async Task<Admin> CreateAdminAsync(Admin admin)
        {
            _dbContext.Admins.Add(admin);
            await _dbContext.SaveChangesAsync();
            return admin;
        }
    }
}
