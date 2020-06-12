using Microsoft.Extensions.Options;
using Repository.Database.Query_and_QueryParam.Interfaces;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Repository.Database.Query_and_QueryParam
{
    public class Update : IUpdate
    {
        private readonly AppSettings _appSettings;
        public Update(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// Updates both the Query and all its associated QueryParams in the database,
        /// with the values from the edit-view, whether or not they were new (changed) or old (untouched) (IT-role needed)
        /// </summary>
        /// <param name="query">all database column values except Id</param>
        /// <param name="queryParams">all database column values except Id</param>
        public void UpdateQuery(Query query, List<QueryParam> queryParams)
        {
            string connectionString = _appSettings.AdministratorConnectionString;

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
                            cmd.Parameters.AddWithValue("@KeyColumn", DBNull.Value);
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
