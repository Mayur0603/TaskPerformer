using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskPerformer.Models
{
    public class User
    {
        [ForeignKey("UserId")]
        public int Id { get; set; } 

        [Required(ErrorMessage = "User Name Is Required")]
        [StringLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public String Username { get; set; }

        [Required(ErrorMessage = "Email Is Required")]
        [StringLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public String Email { get; set; }

        [Required(ErrorMessage = "Password Is Required")]
        [StringLength(255)]
        [Column(TypeName = "nvarchar(255)")]
        public String Password { get; set; }

        [Column(TypeName = "DateTime2")]
        public DateTime CreatDate { get; set; } = DateTime.Now;

        [StringLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public String? FirstName { get; set; }

        [StringLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public String? LastName { get; set; }

        public bool IsActive { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }



    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class EditViewModel
    {
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
    }

    public class UpdateUserResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
