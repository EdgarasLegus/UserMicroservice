using System.ComponentModel.DataAnnotations;

namespace UserApi.Models
{
    public class UserModel
    {

        public int UserId { get; set; }

        [StringLength(25, ErrorMessage = "Must be between 1 and 25 characters", MinimumLength = 2)]
        [Required(ErrorMessage = "FirstName is required")]
        public string FirstName { get; set; }

        [StringLength(25, ErrorMessage = "Must be between 1 and 25 characters", MinimumLength = 2)]
        [Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email should correspond to standards! Example: test@gmail.com")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
    }
}