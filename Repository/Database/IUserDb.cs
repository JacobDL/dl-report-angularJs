using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Database
{
    public interface IUserDb
    {
        public User ValidateUser(AuthenticateModel user);
        public List<AuthenticateModel> GetAllUsers();
    }
}
