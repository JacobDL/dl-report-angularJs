using Microsoft.Extensions.Options;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Repository.Database
{
    public class UserDb : IUserDb
    {
        private readonly AppSettings _appSettings;
        public UserDb(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        /// <summary>
        /// Gets the values of one row in the Administrators Table if both the Username and the Password matches
        /// Used to Authenticate a login request 
        /// </summary>
        /// <param name="user">AuthenticateModel with username and password</param>
        /// <returns>a user with all the values in the class added</returns>
        public User ValidateUser(AuthenticateModel user)
        {
            string connectionString = _appSettings.AdministratorConnectionString;

            string queryString = $"select * from Administrators where Username = @Username and Password = @Password";
            User validUser = new User();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(queryString, connection))
                {
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@Password", user.Password);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            validUser.AdminId = int.Parse(reader["AdminId"].ToString());
                            validUser.Username = reader["Username"].ToString();
                            validUser.Password = reader["Password"].ToString();
                            validUser.Name = reader["Name"].ToString();
                            validUser.RoleId = int.Parse(reader["RoleId"].ToString());
                        }
                    }

                }


            }

            return validUser;
        }

        /// <summary>
        /// Gets all the users from the table "Administrators" but only Username and Password column values
        /// </summary>
        /// <returns>a list of "AuthenticateModel"</returns>
        public List<AuthenticateModel> GetAllUsers()
        {
            string connectionString = _appSettings.AdministratorConnectionString;

            string queryString = $"select * from Administrators";

            List<AuthenticateModel> users = new List<AuthenticateModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(queryString, connection))
                {

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                AuthenticateModel user = new AuthenticateModel
                                {
                                    Username = reader["Username"].ToString(),
                                    Password = reader["Password"].ToString(),
                                };
                                users.Add(user);
                            }

                        }
                    }
                }

            }

            return users;
        }
    }

}
