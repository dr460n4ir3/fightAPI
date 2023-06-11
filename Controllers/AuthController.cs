using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fightAPI.Dtos.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fightAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;
        public AuthController(IAuthRepository authRepo)
        {
            _authRepo = authRepo;
            
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto request)
        {
            if(!IsValidPassword(request.Password)) 
            {
                return BadRequest(new ServiceResponse<int> {
                    Success = false,
                    Message = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number, and one special character."
                });
            }

            var response = await _authRepo.Register(
                new User { Username = request.Username }, request.Password
            );
            if(!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        private bool IsValidPassword(string password) // this is a custom method to check if the password is valid
        {
            var hasNumber = new System.Text.RegularExpressions.Regex(@"[0-9]+"); // this is a regular expression to check if the password has a number
            var hasUpperChar = new System.Text.RegularExpressions.Regex(@"[A-Z]+"); // this is a regular expression to check if the password has an uppercase letter
            var hasLowerChar = new System.Text.RegularExpressions.Regex(@"[a-z]+"); // this is a regular expression to check if the password has a lowercase letter
            var hasMinimum8Chars = new System.Text.RegularExpressions.Regex(@".{8,}"); // this is a regular expression to check if the password has at least 8 characters
            var hasSpecialChar = new System.Text.RegularExpressions.Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]"); // this is a regular expression to check if the password has a special character

            return hasNumber.IsMatch(password) && hasUpperChar.IsMatch(password) && hasLowerChar.IsMatch(password) && hasMinimum8Chars.IsMatch(password) && hasSpecialChar.IsMatch(password);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserLoginDto request)
        {
            var response = await _authRepo.Login(request.Username, request.Password);
            if(!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<ActionResult<ServiceResponse<string>>> ChangePassword(UserChangePasswordDto request)
        {
            var response = await _authRepo.ChangePassword(request.UserId, request.OldPassword, request.NewPassword);
            if(!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpDelete("DeleteUser")]
        public async Task<ActionResult<ServiceResponse<string>>> DeleteUser(UserDeleteDto request)
        {
            var response = await _authRepo.DeleteUser(request.UserId);
            if(!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}