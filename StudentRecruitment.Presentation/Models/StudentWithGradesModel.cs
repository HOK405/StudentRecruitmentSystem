namespace StudentRecruitment.Presentation.Models
{
    public class StudentWithGradesModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronimic { get; set; }
        public string Description { get; set; }
        public DateTime BirthDate { get; set; }
        public List<SemesterGrade> SemesterGrades { get; set; }
    }

    public class SemesterGrade
    {
        public int Semester { get; set; }
        public int Grade { get; set; }
        public string SubjectName { get; set; }
    }
}
