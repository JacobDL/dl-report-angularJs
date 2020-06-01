using Repository.Database.Query_and_QueryParam;
using Repository.Database.Query_and_QueryParam.Interfaces;
using Repository.Models;
using Repository.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Repositories.Query_and_QueryParam
{
    public class QueryRepository : IQueryRepository
    {
        private readonly IDelete _delete;
        private readonly IInsert _insert;
        private readonly ISelectQuery _selectQuery;
        private readonly ISelectQueryParam _selectQueryParam;
        private readonly IUpdate _update;
        public QueryRepository(IDelete delete, IInsert insert, ISelectQuery selectQuery, ISelectQueryParam selectQueryParam, IUpdate update)
        {
            _delete = delete;
            _insert = insert;
            _selectQuery = selectQuery;
            _selectQueryParam = selectQueryParam;
            _update = update;
        }

        public List<Query> GetQueries()
        {
            return _selectQuery.GetQueries();
        }
        public Query GetQueryById(int id)
        {
            return _selectQuery.GetQueryById(id);
        }
        public List<QueryParam> GetQueryParamsByQueryId(int id, bool needSqlList)
        {
            return _selectQueryParam.GetQueryParamsById(id, needSqlList);
        }

        public List<QueryParam> GetAllQueryParams()
        {
            return _selectQueryParam.GetAllQueryParams();
        }

        public void InsertQuery(Query query, List<QueryParam> queryParams)
        {
            _insert.InsertQuery(query, queryParams);
        }

        public void DeleteQueryById(int queryId)
        {
            _delete.DeleteQuery(queryId);
        }

        public void DeleteQueryParamsById(int paramId)
        {
            _delete.DeleteQueryParam(paramId);
        }

        public void InsertQueryParam(List<QueryParam> queryParams, int id)
        {
            _insert.InsertQueryParam(queryParams, id);
        }

        public void UpdateQuery(Query query, List<QueryParam> queryParams)
        {
            _update.UpdateQuery(query, queryParams);
        }
    }
}
