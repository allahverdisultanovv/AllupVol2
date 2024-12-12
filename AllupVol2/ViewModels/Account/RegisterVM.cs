using System.ComponentModel.DataAnnotations;

namespace AllupVol2.ViewModels
{
    public class RegisterVM
    {
        [MaxLength(25)]
        [MinLength(3)]
        public string Name { get; set; }
        [MaxLength(25)]
        [MinLength(3)]
        public string Surname { get; set; }
        [MaxLength(100)]
        [MinLength(4)]
        public string Username { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ComfirmPassword { get; set; }
    }
}
