using Microsoft.Extensions.Options;
using Repository.Database.Query_and_QueryParam.Interfaces;
using Repository.Models;
using Repository.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Repository.Database.Query_and_QueryParam
{
    public class Delete : IDelete
    {
        private readonly AppSettings _appSettings;
        public Delete(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// First Deletes all the QueryParams connected to the chosen Query and then deletes to query (IT-role needed)
        /// </summary>
        /// <param name="id">the chosen QueryId</param>
        public void DeleteQuery(int id)
        {
            string connectionString = _appSettings.AdministratorConnectionString;
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
        public void DeleteQueryParam(int id)
        {
            string connectionString = _appSettings.AdministratorConnectionString;
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
