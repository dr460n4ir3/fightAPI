using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fightAPI.Data
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Register(User user, string password); // this method will register the user
        Task<ServiceResponse<string>> Login(string username, string password); // this method will return a token
        Task<bool> UserExists(string username); // this method will check if the user exists in the database
        Task<ServiceResponse<string>> ChangePassword(int userId, string oldPassword, string newPassword); // this method will change the user's password
        Task<ServiceResponse<string>> DeleteUser(int userId); // this method will delete the user
    }
}