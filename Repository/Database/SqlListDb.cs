using Repository.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Repository.Database
{
    class SqlListDb
    {
        private readonly string connectionString = "Data Source=localhost;Initial Catalog=DreamLogisticsReport;Integrated Security=True";

        internal List<SqlTable> GetSqlList(QueryParam queryParam)
        {
            List<SqlTable> tableList = new List<SqlTable>();

            if (queryParam.TableName != null && queryParam.TableName.Length > 0)
            {
                string queryString = $"SELECT {queryParam.KeyColumn},{queryParam.DisplayColumn} FROM {queryParam.TableName}";


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
                                    SqlTable table = new SqlTable();

                                    //table.Id = int.Parse(reader["Id"].ToString());
                                    table.DisplayColumn = reader[$"{queryParam.DisplayColumn}"].ToString();
                                    table.KeyColumn = reader[$"{queryParam.KeyColumn}"].ToString();

                                    tableList.Add(table);
                                }


                            }
                        }
                    }
                }
            }


            return tableList;
        }
    }
}
