namespace StudentRecruitment.BLL.DTOs
{
    public class StudentImportDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronimic { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string BirthDate { get; set; }
        public List<SemesterDto> Semesters { get; set; }
    }
}
