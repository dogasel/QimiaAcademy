using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implementations.Commands.Users.Dtos
{
    public class CreateUserDto
    {
        
        public string Password { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FirstMidName { get; set; } = string.Empty;
        public UserStatus Status { get; set; }
        

    }
}
