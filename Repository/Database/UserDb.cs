using Repository.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Repository.Database
{
    class UserDb
    {
        private readonly string connectionString = "Data Source=localhost;Initial Catalog=DreamLogisticsReport;Integrated Security=True";

        internal User ValidateUser(AuthenticateModel user)
        {
            string queryString = $"select * from [User] where Username = @Username and Password = @Password";
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
                            validUser.Id = int.Parse(reader["Id"].ToString());
                            validUser.Username = reader["Username"].ToString();
                            validUser.Password = reader["Password"].ToString();
                        }
                    }

                }


            }

            return validUser;
        }
    }
    
}
