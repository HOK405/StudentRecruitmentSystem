using Microsoft.AspNetCore.Identity;

namespace StudentRecruitment.Domain.Entities
{
    public class Student : IdentityUser<int>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronimic { get; set; }
        public string Description { get; set; }
        public bool IsPublicProfile { get; set; }
        public DateTime BirthDate { get; set; }

        public ICollection<SemesterInfo> SemesterInfos { get; set; }
        public ICollection<StudentEmployer> StudentEmployers { get; set; }
    }
}