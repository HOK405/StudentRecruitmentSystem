namespace StudentRecruitment.Domain.Entities
{
    public class SemesterInfo
    {
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public int Semester { get; set; }
        public int Grade { get; set; }

        public virtual Student Student { get; set; }
        public virtual Subject Subject { get; set; }
    }
}