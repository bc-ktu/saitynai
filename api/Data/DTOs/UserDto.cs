using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace api.Data.DTOs
{
    public class CreateUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class SuccessfulLoginResponseDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }

    /* public class RegisterUserDto
     {
         [Required]
         public string FirstName { get; set; }
         [Required]
         public string LastName { get; set; }
         [Required]
         [EmailAddress]
         public string Email { get; set; }
         [Required]
         public string Phone { get; set; }

     }*/
    public class LoginUserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Address { get; set; }
        public bool IsApproved { get; set; }
        public bool HasFinishedRegistration { get; set; }
    }
    public class ApproveByAdminUserDto
    {
        public Guid Id { get; set; }
        public string PasswordHash { get; set; }
        public bool IsApproved { get; set; }
    }
    public class HasFinishedRegistrationUserDto
    {
        public Guid Id { get; set; }
        public bool HasFinishedRegistration { get; set; }
    }
    public class UpdateUserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Address { get; set; }
        public string PasswordHash { get; set; }
    }
}
