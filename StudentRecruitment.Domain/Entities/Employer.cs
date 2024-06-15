using Microsoft.AspNetCore.Identity;

namespace StudentRecruitment.Domain.Entities
{
    public class Employer : IdentityUser<int>
    {
        public string CompanyName { get; set; }
        public string Location { get; set; }
        public string Phone { get; set; }

        public ICollection<StudentEmployer> StudentEmployers { get; set; }
    }
}