using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens; // this is needed for the SecurityTokenDescriptor class



namespace fightAPI.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;
        public AuthRepository(DataContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            var response = new ServiceResponse<string>();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username.ToLower() == username.ToLower()); // this will get the user from the database
            if(user is null)
            {
                response.Success = false;
                response.Message = "User not found."; // Note: message will expose information to the user, so it should be vague
            }
            else if(!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Incorrect password.";
            }
            else
            {
                response.Data = CreateToken(user);
            }

            return response;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            var response = new ServiceResponse<int>();
            if(await UserExists(user.Username))
            {
                response.Success = false;
                response.Message = "User already exists.";
                return response;
            }
            
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt); // this will create a hash and a salt
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            response.Data = user.Id;
            return response;
        }

        public async Task<bool> UserExists(string username)
        {
            if(await _context.Users.AnyAsync(x => x.Username.ToLower() == username.ToLower()))
            {
                return true;
            }
            return false;
        }

        public async Task<ServiceResponse<string>> ChangePassword(int userId, string oldPassword, string newPassword)
        {
            var response = new ServiceResponse<string>();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user is null)
            {
                response.Success = false;
                response.Message = "User not found.";
            }
            else if (!VerifyPasswordHash(oldPassword, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Incorrect password.";
            }
            else
            {
                CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                await _context.SaveChangesAsync();
                response.Data = "Password changed successfully.";
            }

            return response;
        }

        public async Task<ServiceResponse<string>> DeleteUser(int userId)
        {
            var response = new ServiceResponse<string>();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user is null)
            {
                response.Success = false;
                response.Message = "User not found.";
            }
            else
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                response.Data = "User deleted successfully.";
            }

            return response;
        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512()) // this is a built in class that will generate a random key
            {
                passwordSalt = hmac.Key; // this will generate a random key
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); // this will generate a hash using the key and the password
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)) // this will use the key to generate a hash
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); // this will generate a hash using the key and the password
                return computedHash.SequenceEqual(passwordHash); // this will compare the two hashes
            }
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim> // this will create a list of claims
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // this will create a claim for the user's id
                new Claim(ClaimTypes.Name, user.Username) // this will create a claim for the user's username
            };

            var appSettingsToken = _config.GetSection("AppSettings:Token").Value; // this will get the token from appsettings.json
            if(appSettingsToken is null)
            {
                throw new ArgumentNullException("AppSettings:Token", "Token not found in appsettings.json");
            }

            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(appSettingsToken)); // this will create a key using the token
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature); // this will create the credentials using the key and the algorithm

            var tokenDescriptor = new SecurityTokenDescriptor // this will create the token descriptor
            {
                Subject = new ClaimsIdentity(claims), // this will add the claims to the token descriptor
                // set expiration date of token in minutes
                //Expires = DateTime.Now.AddMinutes(10), // this will set the expiration date of the token in minutes

                Expires = DateTime.Now.AddDays(1), // this will set the expiration date of the token in days
                SigningCredentials = creds // this will add the credentials to the token descriptor
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler(); // this will create the token handler
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor); // this will create the token using the token descriptor
            
            return tokenHandler.WriteToken(token); // this will return the token
        }
    }
}