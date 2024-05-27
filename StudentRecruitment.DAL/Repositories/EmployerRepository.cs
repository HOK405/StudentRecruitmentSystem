using StudentRecruitment.DAL.Interfaces;
using StudentRecruitment.Domain.Entities;

namespace StudentRecruitment.DAL.Repositories
{
    public class EmployerRepository : IEmployerRepository
    {
        private readonly ApidDbContext _dbContext;

        public EmployerRepository(ApidDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Employer> GetEmployerByIdAsync(int employerId)
        {
            return await _dbContext.Employers.FindAsync(employerId);
        }
    }
}
