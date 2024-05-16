using System.ComponentModel.DataAnnotations;

namespace StudentRecruitment.Presentation.Models
{
    public class StudentIdInputModel
    {
        [Required(ErrorMessage = "Student ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Student ID must be a positive integer")]
        public int? StudentId { get; set; }
    }
}
