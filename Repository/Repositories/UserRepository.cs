using Repository.Database;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Repositories
{
    public class UserRepository
    {
        public static User ValidateUser(AuthenticateModel user)
        {
            UserDb db = new UserDb();
            return db.ValidateUser(user);
        }

        public static List<AuthenticateModel> GetAllUsers()
        {
            UserDb db = new UserDb();
            return db.GetAllUsers();
        }
    }
}
