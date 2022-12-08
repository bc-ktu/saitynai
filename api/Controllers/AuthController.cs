using api.Data.DTOs;
using api.Data.Entities;
using api.Data.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PasswordGenerator;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.Collections.Generic;
using api.Entities;
using System.Security.Claims;
using api.Authorization.Model;
using EllipticCurve.Utils;
using api.DTOs;

namespace api.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<RegisteredUser> userManager;
        private readonly IMapper mapper;
        private readonly IJwtTokenService tokenService;
        private readonly IConfiguration configuration;
        private readonly IAuthorizationService authorizationService;

        private Password pswGenerator = new Password(includeLowercase: true, includeUppercase: true, includeNumeric: true, includeSpecial: true, passwordLength: 16);

        public AuthController(UserManager<RegisteredUser> userManager, IMapper mapper, IJwtTokenService tokenService, IConfiguration configuration, IAuthorizationService authorizationService)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.tokenService = tokenService;
            this.configuration = configuration;
            this.authorizationService = authorizationService;
        }

        [HttpPost]
        [Route("api/Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register (CreateUserDto registerUserDto) 
        {
            var user = await userManager.FindByEmailAsync(registerUserDto.Email);
            if (user != null)
                return BadRequest("Vartotojas su šiuo elektroniniu paštu egzistuoja");

            var newUser = new RegisteredUser
            {
                Email = registerUserDto.Email,
                FirstName = registerUserDto.FirstName,
                LastName = registerUserDto.LastName,
                PhoneNumber = registerUserDto.PhoneNumber,
                IsApproved = false,
                UserName = registerUserDto.Email,
                Address = "",
                City = "",
                ZipCode = "",
                HasFinishedRegistration = false                
            };
            var createUserResult = await userManager.CreateAsync(newUser);
            if (!createUserResult.Succeeded)
                return BadRequest("Nepavyko sukurti vartotojo.");
            await userManager.AddToRoleAsync(newUser, Roles.RegisteredUser.ToString());
            
            return Ok(CreatedAtAction(nameof(Register), mapper.Map<RegisteredUser, CreateUserDto>(newUser)));
        }

        [HttpPost]
        [Route("api/Token")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginUserDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                return BadRequest("Neteisingas elektroninis paštas arba slaptažodis.");

            var isPasswordValid = await userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordValid)
                return BadRequest("Neteisingas elektroninis paštas arba slaptažodis.");

            if (!user.IsApproved)
                return BadRequest("Vartotojas nėra patvirtintas.");

            var roles = await userManager.GetRolesAsync(user);
            var accessToken = tokenService.CreateAccessToken(user.Id, user.Email, roles);

            var succesfulLoginDto = new SuccessfulLoginResponseDto();
            succesfulLoginDto.AccessToken = accessToken;
            var userData = mapper.Map<RegisteredUser, UserDto>(user);
            succesfulLoginDto.UserData = userData;
          //  succesfulLoginDto.RefreshToken = refreshToken;

            return Ok(succesfulLoginDto);
        }

        [HttpGet]
        [Route("api/Users")]
        [Authorize(Roles=Roles.Admin)]
        public async Task<ActionResult<List<UserDto>>> GetRegisteredUsers()
        {
            List<RegisteredUser> users = (List<RegisteredUser>)await userManager.GetUsersInRoleAsync(Roles.RegisteredUser);
            return Ok(mapper.Map<List<RegisteredUser>, List<UserDto>>(users));
        }

        [HttpPut]
        [Route("api/UserUpdate")]
        [Authorize(Roles=Roles.Admin)]
        public async Task<ActionResult<UserDto>> UpdateUser(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound("Nerastas vartotojas.");

            var psw = pswGenerator.Next();
            await userManager.AddPasswordAsync(user, psw);
            user.IsApproved = true;

            var isUpdated = await userManager.UpdateAsync(user);

            if (!isUpdated.Succeeded)
                return BadRequest("Nepavyko išsaugoti.");

            await Execute(user, psw);

            return Ok(mapper.Map<RegisteredUser, UserDto>(user));
        }

        [HttpPut]
        [Route("api/PasswordReset")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> ChangePassword(PasswordResetUserDto passwordResetUserDto)
        {
            var user = await userManager.FindByEmailAsync(passwordResetUserDto.Email);
            if (user == null)
                return NotFound("Neteisingas elektroninis paštas arba slaptažodis.");

            var isPasswordValid = await userManager.CheckPasswordAsync(user, passwordResetUserDto.OldPassword);
            if (!isPasswordValid)
                return NotFound("Neteisingas elektroninis paštas arba slaptažodis.");

            if (!user.IsApproved)
                return BadRequest("Vartotojas nėra patvirtintas.");

            var change = await userManager.ChangePasswordAsync(user, passwordResetUserDto.OldPassword, passwordResetUserDto.NewPassword);
            if(!change.Succeeded)
                return BadRequest("Nepavyko pakeisti slaptažodžio.");

            var isUpdated = await userManager.UpdateAsync(user);
            if (!isUpdated.Succeeded)
                return BadRequest("Nepavyko išsaugoti.");

            user.HasFinishedRegistration = true;

            return Ok(mapper.Map<RegisteredUser, UserDto>(user));
        }


        [HttpPut]
        [Route("api/Profile")]
        [Authorize(Roles = Roles.Admin + "," + Roles.RegisteredUser)]
        public async Task<ActionResult<UserDto>> UpdateProfile(UpdateUserDto userDto)
        {
            var email = User.FindFirstValue(ClaimTypes.NameIdentifier);

            /*var authorizationResult = await authorizationService.AuthorizeAsync(User, userDto.Email, PolicyNames.ResourceOwner);
            if (!authorizationResult.Succeeded)
                return Forbid();*/

            var user = await userManager.FindByEmailAsync(userDto.Email);
            if (user == null)
                return NotFound("Nerastas vartotojas.");

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.PhoneNumber = userDto.PhoneNumber;
            user.Address = userDto.Address;
            user.ZipCode = userDto.ZipCode;
            user.City = userDto.City;

            await userManager.UpdateAsync(user);
            
            return Ok(user);
        }

        private async Task Execute(RegisteredUser user, string psw)
        {
            var apiKey = configuration.GetValue<string>("Sendgrid");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("beatrice.ceplaite@gmail.com", "Admin User");
            var subject = "Sveiki prisijungę!";
            var to = new EmailAddress(user.Email);
            var plainTextContent = $"Jūsų laikinas slaptažodis: {psw}";
            var htmlContent = "";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
