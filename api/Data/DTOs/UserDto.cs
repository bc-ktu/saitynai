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

    public class LoginUserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserDto
    {
        public string Id { get; set; }
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
    public class PasswordResetUserDto
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
    }
    public class HasFinishedRegistrationUserDto
    {
        public string Email { get; set; }
        public bool HasFinishedRegistration { get; set; }
    }
    public class UpdateUserDto
    {
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
