using Microsoft.AspNetCore.Identity;

namespace EmployeesClasses01.Models
{
    public class ApplicationUser : IdentityUser
    {

        [PersonalData]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }

    }
}
