using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Models
{
    public class User
    {
        public int AdminId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public int RoleId { get; set; }
        public string Token { get; set; }
    }
}
