using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Repositories
{
    public interface IUserRepository
    {
        public User ValidateUser(AuthenticateModel user);

        public List<AuthenticateModel> GetAllUsers();
    }
}
