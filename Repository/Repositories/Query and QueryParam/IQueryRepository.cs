using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Repositories.Query_and_QueryParam
{
    public interface IQueryRepository
    {
        public List<Query> GetQueries();
        public Query GetQueryById(int id);
        public List<QueryParam> GetQueryParamsByQueryId(int id, bool needSqlList);

        public List<QueryParam> GetAllQueryParams();

        public void InsertQuery(Query query, List<QueryParam> queryParams);

        public void DeleteQueryById(int queryId);

        public void DeleteQueryParamsById(int paramId);

        public void InsertQueryParam(List<QueryParam> queryParams, int id);

        public void UpdateQuery(Query query, List<QueryParam> queryParams);
    }
}
