using Repository.Database.Query_and_QueryParam;
using Repository.Models;
using Repository.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Repositories.Query_and_QueryParam
{
    public class QueryRepository
    {
        public static List<Query> GetQueries()
        {
            SelectQuery db = new SelectQuery();
            return db.GetQueries();
        }
        public static Query GetQueryById(int id)
        {
            SelectQuery db = new SelectQuery();
            return db.GetQueryById(id);
        }
        public static List<QueryParam> GetQueryParamsByQueryId(int id, bool needSqlList)
        {
            SelectQueryParam db = new SelectQueryParam();
            return db.GetQueryParamsById(id, needSqlList);
        }

        public static List<QueryParam> GetAllQueryParams()
        {
            SelectQueryParam db = new SelectQueryParam();
            return db.GetAllQueryParams();
        }

        public static void InsertQuery(Query query, List<QueryParam> queryParams)
        {
            Insert db = new Insert();
            db.InsertQuery(query, queryParams);
        }

        public void DeleteQueryById(int queryId)
        {
            Delete db = new Delete();
            db.DeleteQuery(queryId);
        }

        public static void DeleteQueryParamsById(int paramId)
        {
            Delete db = new Delete();
            db.DeleteQueryParam(paramId);
        }

        public static void InsertQueryParam(List<QueryParam> queryParams, int id)
        {
            Insert db = new Insert();
            db.InsertQueryParam(queryParams, id);
        }

        public static void UpdateQuery(Query query, List<QueryParam> queryParams)
        {
            Update db = new Update();
            db.UpdateQuery(query, queryParams);
        }
    }
}
