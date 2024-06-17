namespace StudentRecruitment.Shared.DTOs
{
    public class ChangePasswordDto
    {
        public string Username { get; set; }
        public string NewPassword { get; set; }
        public string UserType { get; set; }
    }
}
