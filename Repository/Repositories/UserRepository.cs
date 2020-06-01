using Repository.Database;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IUserDb _userDb;
        public UserRepository(IUserDb userDb)
        {
            _userDb = userDb;
        }
        public User ValidateUser(AuthenticateModel user)
        {
            return _userDb.ValidateUser(user);
        }

        public List<AuthenticateModel> GetAllUsers()
        {
            return _userDb.GetAllUsers();
        }
    }
}
