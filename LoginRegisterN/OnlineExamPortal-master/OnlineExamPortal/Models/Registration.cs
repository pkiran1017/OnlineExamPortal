using System.ComponentModel.DataAnnotations;

namespace OnlineExamPortal.Models
{
    public class Registration
    {
        
        public int UserId { get; set; }
        [Required(ErrorMessage ="Username must be there")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password must be there")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Email must be there")]

        public string Email { get; set; }
        [Required(ErrorMessage = "Mobile must be there")]


        public string Mobile { get; set; }
        [Required(ErrorMessage = "Userrole must be there")]
        public string UserRole { get; set; }

    }
}
