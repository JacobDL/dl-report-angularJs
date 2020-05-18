using Repository.Models;
using Repository.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Repository.Database.Query_and_QueryParam
{
    class Insert
    {
        private readonly string connectionString = "Data Source=localhost;Initial Catalog=DreamLogisticsReport;Integrated Security=True";

        internal void InsertQuery(Query query, List<QueryParam> queryParams)
        {
            var queryId = 0;
            string queryString = $"INSERT INTO [Query] ([Name], [Sql]) " +
                    $"VALUES (@name, @sql); SELECT CONVERT(int, SCOPE_IDENTITY());";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(queryString, connection))
                {
                    cmd.Parameters.AddWithValue("@Name", query.GroupName);
                    cmd.Parameters.AddWithValue("@Sql", query.Sql);
                    queryId = (int)cmd.ExecuteScalar();
                }
            };
            InsertQueryParam(queryParams, queryId);
        }

        public void InsertQueryParam(List<QueryParam> queryParams, int queryId)
        {
            string queryString = $"INSERT INTO QueryParam (QueryId, [Name], TypeId, ParameterCode, TableName, ColumnName, KeyColumn, ParamOptional) " +
                   $"VALUES (@QueryId, @Name, @TypeId, @ParameterCode, @TableName, @ColumnName, @KeyColumn, @ParamOptional);";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (var queryParam in queryParams)
                {
                    using (SqlCommand cmd = new SqlCommand(queryString, connection))
                    {
                        cmd.Parameters.AddWithValue("@QueryId", queryId);
                        cmd.Parameters.AddWithValue("@Name", queryParam.Name);
                        cmd.Parameters.AddWithValue("@TypeId", queryParam.TypeId);
                        cmd.Parameters.AddWithValue("@ParameterCode", queryParam.ParameterCode);
                        cmd.Parameters.AddWithValue("@ParamOptional", queryParam.ParamOptional);

                        if (queryParam.TableName != null && queryParam.TableName.Length > 0)
                        {
                            cmd.Parameters.AddWithValue("@TableName", queryParam.TableName);
                            cmd.Parameters.AddWithValue("@ColumnName", queryParam.DisplayColumn);
                            cmd.Parameters.AddWithValue("@KeyColumn", queryParam.KeyColumn);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@TableName", DBNull.Value);
                            cmd.Parameters.AddWithValue("@ColumnName", DBNull.Value);
                            cmd.Parameters.AddWithValue("@KeyColumn", DBNull.Value);
                        }


                        cmd.ExecuteNonQuery();
                    };
                }
            }
        }
    }
}
