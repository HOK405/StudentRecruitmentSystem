using Microsoft.EntityFrameworkCore;
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

        public async Task LikeStudentAsync(int employerId, int studentId)
        {
            var studentEmployer = new StudentEmployer
            {
                EmployerId = employerId,
                StudentId = studentId
            };
            _dbContext.StudentEmployers.Add(studentEmployer);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Student>> GetLikedStudentsAsync(int employerId)
        {
            return await _dbContext.StudentEmployers
                .Where(se => se.EmployerId == employerId)
                .Select(se => se.Student)
                .ToListAsync();
        }

        public async Task DislikeStudentAsync(int employerId, int studentId)
        {
            var studentEmployer = await _dbContext.StudentEmployers
                .FirstOrDefaultAsync(se => se.EmployerId == employerId && se.StudentId == studentId);
            if (studentEmployer != null)
            {
                _dbContext.StudentEmployers.Remove(studentEmployer);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}