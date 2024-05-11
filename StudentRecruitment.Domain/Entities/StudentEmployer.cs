namespace StudentRecruitment.Domain.Entities
{
    public class StudentEmployer
    {
        public int StudentId { get; set; }
        public int EmployerId { get; set; }

        public virtual Student Student { get; set; }
        public virtual Employer Employer { get; set; }
    }
}