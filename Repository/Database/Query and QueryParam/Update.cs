using Repository.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Repository.Database.Query_and_QueryParam
{
    class Update
    {
        private string connectionString = "Data Source=localhost;Initial Catalog=DreamLogisticsReport;Integrated Security=True";

        internal void UpdateQuery(Query query, List<QueryParam> queryParams)
        {
            string queryParamString = "UPDATE QueryParam SET [Name]=@Name, TypeId=@TypeId, ParameterCode=@ParameterCode," +
                " TableName=@TableName, ColumnName=@ColumnName, KeyColumn=@KeyColumn, ParamOptional=@ParamOptional WHERE Id=@Id";
            string queryString = "UPDATE Query SET [Name] = @name, [Sql]= @Sql WHERE Id=@Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                foreach (var queryParam in queryParams)
                {
                    using (SqlCommand cmd = new SqlCommand(queryParamString, connection))
                    {
                        cmd.Parameters.AddWithValue("@Name", queryParam.Name);
                        cmd.Parameters.AddWithValue("@ParameterCode", queryParam.ParameterCode);
                        cmd.Parameters.AddWithValue("@TypeId", queryParam.TypeId);
                        cmd.Parameters.AddWithValue("@ParamOptional", queryParam.ParamOptional);
                        cmd.Parameters.AddWithValue("@Id", queryParam.Id);
                        if (queryParam.TableName != null)
                        {
                            cmd.Parameters.AddWithValue("@TableName", queryParam.TableName);
                            cmd.Parameters.AddWithValue("@ColumnName", queryParam.DisplayColumn);
                            cmd.Parameters.AddWithValue("@KeyColumn", queryParam.KeyColumn);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@TableName", DBNull.Value);
                            cmd.Parameters.AddWithValue("@ColumnName", DBNull.Value);
                        }

                        cmd.ExecuteNonQuery();
                    }
                }
                using (SqlCommand cmd2 = new SqlCommand(queryString, connection))
                {
                    cmd2.Parameters.AddWithValue("@Name", query.GroupName);
                    cmd2.Parameters.AddWithValue("@Sql", query.Sql);
                    cmd2.Parameters.AddWithValue("@Id", query.Id);
                    cmd2.ExecuteNonQuery();
                }

            }
        }
    }
}
