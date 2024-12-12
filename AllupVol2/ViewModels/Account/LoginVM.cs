using System.ComponentModel.DataAnnotations;

namespace AllupVol2.ViewModels
{
    public class LoginVM
    {
        [MinLength(4)]
        [MaxLength(256)]
        public string UserOrEmail { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsPersistant { get; set; }
    }
}
