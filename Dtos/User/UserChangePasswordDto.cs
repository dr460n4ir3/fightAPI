using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fightAPI.Dtos.User
{
    public class UserChangePasswordDto
    {
        public int UserId { get; set; }
        public string OldPassword { get; set; } // this is the old password
        public string NewPassword { get; set; } // this is the new password
    }
}