namespace StudentRecruitment.BLL.DTOs.Output
{
    public class StudentWithGradesOutputModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronimic { get; set; }
        public string Description { get; set; }
        public DateTime BirthDate { get; set; }
        public List<SemesterGrade> SemesterGrades { get; set; }
    }
}
