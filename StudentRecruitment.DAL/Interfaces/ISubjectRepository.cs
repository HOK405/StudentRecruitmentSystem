using StudentRecruitment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentRecruitment.DAL.Interfaces
{
    public interface ISubjectRepository
    {
        Task AddSubjectsAsync(IEnumerable<Subject> subjects);
        Task SaveChangesAsync();
    }
}
