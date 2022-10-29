using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace api.Data.Entities
{
    public class RegisteredUser : IdentityUser
    {
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        [Required]
        [ProtectedPersonalData]
        public string FirstName { get; set; }
        [Required]
        [ProtectedPersonalData]
        public string LastName { get; set; }
        public bool IsApproved { get; set; }
        public bool HasFinishedRegistration { get; set; }

    }
}
