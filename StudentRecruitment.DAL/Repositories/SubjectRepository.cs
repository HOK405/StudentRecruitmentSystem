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

        public IQueryable<Subject> GetSubjects()
        {
            return _context.Subjects.AsQueryable();
        }

        public async Task<List<Subject>> GetSubjectsByNameAsync(string name)
        {
            return await _context.Subjects
                                 .Where(s => s.Name.Contains(name))
                                 .ToListAsync();
        }

        public async Task DeleteAllSubjectsAsync()
        {
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM Subjects");
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}