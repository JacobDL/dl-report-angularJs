using Repository.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Repository.Database.Query_and_QueryParam
{
    class Delete
    {
        private readonly string connectionString = "Data Source=localhost;Initial Catalog=DreamLogisticsReport;Integrated Security=True";

        /// <summary>
        /// First Deletes all the QueryParams connected to the chosen Query and then deletes to query (IT-role needed)
        /// </summary>
        /// <param name="id">the chosen QueryId</param>
        internal void DeleteQuery(int id)
        {
            string queryString = "DELETE FROM QueryParam WHERE QueryId=@id; DELETE FROM Query WHERE Id=@id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(queryString, connection))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                };
            }
        }

        /// <summary>
        /// Deletes a single QueryParam (IT-role needed)
        /// </summary>
        /// <param name="id">The chosen QueryParamId</param>
        internal void DeleteQueryParam(int id)
        {
            string queryString = "DELETE FROM QueryParam WHERE Id=@id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(queryString, connection))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                };
            }
        }
    }
}
