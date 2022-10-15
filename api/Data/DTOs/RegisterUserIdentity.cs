using Microsoft.AspNetCore.Identity;

namespace api.Data.DTOs
{
    public class RegisterUserIdentity : IdentityUser<Guid>
    {
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
