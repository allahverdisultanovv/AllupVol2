using Microsoft.AspNetCore.Identity;

namespace AllupVol2.Models
{
    public class AppUser:IdentityUser
    {
        public string Name{ get; set; }
        public string Surname{ get; set; }
        
    }
}
