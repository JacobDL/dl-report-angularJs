using Microsoft.Extensions.Options;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Repository.Database
{
    public class SqlListDb : ISqlListDb
    {
        private readonly AppSettings _appSettings;
        public SqlListDb(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        /// <summary>
        /// Gets the column that is used to be the key and the column that is used to be displayed to the user
        /// from the Table matching the QueryParam.TableName value
        /// </summary>
        /// <param name="queryParam">KeyColumn, DisplayColumn and TableName is used</param>
        /// <returns>a list of SqlTable (the two columns of the entire TableName table)</returns>
        public List<SqlTable> GetSqlList(QueryParam queryParam)
        {
            string connectionString = _appSettings.AdministratorConnectionString;

            List<SqlTable> tableList = new List<SqlTable>();

            if (queryParam.TableName != null && queryParam.TableName.Length > 0)
            {
                string queryString = $"SELECT {queryParam.KeyColumn},{queryParam.DisplayColumn} FROM {queryParam.TableName}";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(queryString, connection))
                    {
                        try
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        SqlTable table = new SqlTable();

                                        table.DisplayColumn = reader[$"{queryParam.DisplayColumn}"].ToString();
                                        table.KeyColumn = reader[$"{queryParam.KeyColumn}"].ToString();

                                        tableList.Add(table);
                                    }
                                }
                            }
                        }
                        catch
                        {
                            
                        }
                    }
                }
            }
            return tableList;
        }
    }
}
