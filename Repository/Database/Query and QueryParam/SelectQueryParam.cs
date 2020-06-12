using Microsoft.Extensions.Options;
using Repository.Database.Query_and_QueryParam.Interfaces;
using Repository.Models;
using Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Repository.Database.Query_and_QueryParam
{
    public class SelectQueryParam : ISelectQueryParam
    {
        private readonly AppSettings _appSettings;
        private readonly ISqlListRepository _sqlListRepository;
        public SelectQueryParam(IOptions<AppSettings> appSettings, ISqlListRepository sqlListRepository)
        {
            _appSettings = appSettings.Value;
            _sqlListRepository = sqlListRepository;
        }

        /// <summary>
        /// Gets all the QueryParams with the chosen queryId and also the requested sql-lists from the database
        /// </summary>
        /// <param name="id">QueryId chosen by the user</param>
        /// <param name="needSqlList">a bool that determines whether or not to get the sql-lists</param>
        /// <returns>a list of QueryParams</returns>
        public List<QueryParam> GetQueryParamsById(int id, bool needSqlList)
        {
            string connectionString = _appSettings.UserConnectionString;

            List<QueryParam> queryParams = new List<QueryParam>();

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand cmd = new SqlCommand($"SELECT * FROM QueryParam where QueryId=@Id", connection);
            cmd.Parameters.AddWithValue("@Id", id);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    QueryParam query = new QueryParam
                    {
                        Id = int.Parse(reader["Id"].ToString()),
                        Name = reader["Name"].ToString(),
                        TypeId = int.Parse(reader["TypeId"].ToString()),
                        ParameterCode = reader["ParameterCode"].ToString(),
                        TableName = reader["TableName"].ToString(),
                        DisplayColumn = reader["ColumnName"].ToString(),
                        KeyColumn = reader["KeyColumn"].ToString(),
                        ParamOptional = bool.Parse(reader["ParamOptional"].ToString())

                    };
                    if (query.TypeId == 4 && needSqlList == true)
                    {
                        query.SqlLists = _sqlListRepository.GetSqlList(query);
                    }
                    queryParams.Add(query);

                    
                }

            }
            reader.Close();
            return queryParams;
        }

        /// <summary>
        /// Gets all the column values of all the QueryParams from the database, Only used for the detail-view so far
        /// </summary>
        /// <returns>A list of all the QueryParams</returns>
        public List<QueryParam> GetAllQueryParams()
        {
            string connectionString = _appSettings.UserConnectionString;

            List<QueryParam> queryParams = new List<QueryParam>();

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand cmd = new SqlCommand($"SELECT * FROM QueryParam", connection);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    QueryParam query = new QueryParam
                    {
                        Id = int.Parse(reader["Id"].ToString()),
                        Name = reader["Name"].ToString(),
                        QueryId = int.Parse(reader["QueryId"].ToString()),
                        TypeId = int.Parse(reader["TypeId"].ToString()),
                        ParameterCode = reader["ParameterCode"].ToString(),
                        TableName = reader["TableName"].ToString(),
                        DisplayColumn = reader["ColumnName"].ToString(),
                        KeyColumn = reader["KeyColumn"].ToString(),
                        ParamOptional = bool.Parse(reader["ParamOptional"].ToString())

                    };
                    queryParams.Add(query);
                }

            }
            reader.Close();
            return queryParams;
        }
    }
}
