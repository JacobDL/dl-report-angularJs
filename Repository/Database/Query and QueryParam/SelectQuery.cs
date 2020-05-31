using Repository.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Repository.Database.Query_and_QueryParam
{
    class SelectQuery
    {
        private readonly string connectionString = "Data Source=localhost;Initial Catalog=DreamLogisticsReport;Integrated Security=True";

        /// <summary>
        /// Gets all the Queries from the database (used in the queryList.html)
        /// </summary>
        /// <returns>A list of Queries</returns>
        internal List<Query> GetQueries()
        {
            string queryString = $"SELECT * FROM Query";
            List<Query> queries = new List<Query>();

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
                                Query query = new Query();
                                query.Id = int.Parse(reader["Id"].ToString());
                                query.GroupName = reader["Name"].ToString();
                                query.Sql = reader["Sql"].ToString();
                                queries.Add(query);
                            }
                        }
                    }

                }

            }
            return queries;
        }

        /// <summary>
        /// Gets one Query by its Id
        /// </summary>
        /// <param name="id">the Id chosen by the user</param>
        /// <returns>All the columns value of the Query</returns>
        internal Query GetQueryById(int id)
        {
            string queryString = $"select * from Query where Id=@id";
            Query query = new Query();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(queryString, connection))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            query.Id = int.Parse(reader["Id"].ToString());
                            query.GroupName = reader["Name"].ToString();
                            query.Sql = reader["Sql"].ToString();
                        }
                    }

                }
            }
            return query;
        }
    }
}
