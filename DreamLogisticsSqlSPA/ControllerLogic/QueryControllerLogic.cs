using Microsoft.Extensions.Options;
using Repository.Models;
using Repository.Repositories;
using Repository.Repositories.Query_and_QueryParam;
using Repository.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DreamLogisticsSqlSPA.ControllerLogic
{
    public class QueryControllerLogic
    {
        private readonly IQueryRepository _queryRepository;
        public QueryControllerLogic(IQueryRepository queryRepository)
        {
            _queryRepository = queryRepository;
        }
        //Gets a Query by QueryId and the QueryParams that belongs to it
        public QueryAndQueryParamsViewModel GetQueryViewModel(int id, bool needSqlList)
        {
            QueryAndQueryParamsViewModel svm = new QueryAndQueryParamsViewModel();
            svm.Query = _queryRepository.GetQueryById(id);
            svm.QueryParams = _queryRepository.GetQueryParamsByQueryId(id, needSqlList);
            return svm;
        }

        //The overall function to update a Query
        //Deletes chosen QueryParams, Inserts newly added QueryParams and updates all other changes in the tables [Query] and [QueryParam]
        internal void UpdateQuery(QueryAndQueryParamsViewModel updateModel)
        {
            foreach (int param in updateModel.QueryParamsToDelete)
            {
                _queryRepository.DeleteQueryParamsById(param);
            }

            List<QueryParam> newQueryParams = new List<QueryParam>();
            List<QueryParam> queryParamsToUpdate = new List<QueryParam>();

            foreach (var param in updateModel.QueryParams)
            {
                if (param.Id == 0)
                {
                    newQueryParams.Add(param);
                }
                else
                {
                    queryParamsToUpdate.Add(param);
                }

            }
            _queryRepository.InsertQueryParam(newQueryParams, updateModel.Query.Id);
            _queryRepository.UpdateQuery(updateModel.Query, queryParamsToUpdate);
        }

        
    }
}
