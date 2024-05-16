using Microsoft.EntityFrameworkCore;
using StudentRecruitment.DAL.Interfaces;
using StudentRecruitment.Domain.Entities;

namespace StudentRecruitment.DAL.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly ApidDbContext _context;

        public SubjectRepository(ApidDbContext context)
        {
            _context = context;
        }

        public async Task AddSubjectsAsync(IEnumerable<Subject> subjects)
        {
            foreach (var subject in subjects)
            {
                if (!await _context.Subjects.AnyAsync(s => s.Id == subject.Id))
                {
                    await _context.Subjects.AddAsync(subject);
                }
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public IQueryable<Subject> GetSubjects()
        {
            return _context.Subjects.AsQueryable();
        }
    }

}