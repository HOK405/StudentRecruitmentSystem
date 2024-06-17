using StudentRecruitment.Domain.Entities;

namespace StudentRecruitment.DAL.Interfaces
{
    public interface ISubjectRepository
    {
        Task AddSubjectsAsync(IEnumerable<Subject> subjects);
        IQueryable<Subject> GetSubjects();

        Task<List<Subject>> GetSubjectsByNameAsync(string name);

        Task DeleteAllSubjectsAsync();

        Task SaveChangesAsync();
    }
}