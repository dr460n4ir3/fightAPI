using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fightAPI.Data
{
    public class AuthRepository : IAuthRepository
    {
        public DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;
            
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
                response.Data = user.Id.ToString();
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
    }
}