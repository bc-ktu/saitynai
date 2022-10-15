using api.Data.DTOs;
using api.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [AllowAnonymous]
    //[Authorize]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<RegisterUserIdentity> userManager;
        public AuthController(UserManager<RegisterUserIdentity> userManager)
        {
            userManager = userManager;
        }

        public async Task<IActionResult> Register (RegisterUserDto registerUserDto) 
        {
            var user = await userManager.FindByEmailAsync(registerUserDto.Email);
            if (user == null)
                return BadRequest("Vartotojas su šiuo elektroniniu paštu egzistuoja");

            var newUser = new RegisterUserIdentity
            {
                Email = registerUserDto.Email,
                FirstName = registerUserDto.FirstName,
                LastName = registerUserDto.LastName,
                PhoneNumber = registerUserDto.Phone
            };

            var createUserResult = await userManager.CreateAsync(newUser, "generateUserPSW");
            return Ok(registerUserDto);
        }
    }
}
