namespace StudentRecruitment.Shared.DTOs
{
    public class CreateEmployerDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string CompanyName { get; set; }
        public string Location { get; set; }
        public string Phone { get; set; }
    }
}