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
        //Gets a Query by QueryId and the QueryParams that belongs to it
        public static QueryAndQueryParamsViewModel GetQueryViewModel(int id, bool needSqlList)
        {
            QueryAndQueryParamsViewModel svm = new QueryAndQueryParamsViewModel();
            svm.Query = QueryRepository.GetQueryById(id);
            svm.QueryParams = QueryRepository.GetQueryParamsByQueryId(id, needSqlList);
            return svm;
        }

        //The overall function to update a Query
        //Deletes chosen QueryParams, Inserts newly added QueryParams and updates all other changes in the tables [Query] and [QueryParam]
        internal static void UpdateQuery(QueryAndQueryParamsViewModel updateModel)
        {
            foreach (int param in updateModel.QueryParamsToDelete)
            {
                QueryRepository.DeleteQueryParamsById(param);
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
            QueryRepository.InsertQueryParam(newQueryParams, updateModel.Query.Id);
            QueryRepository.UpdateQuery(updateModel.Query, queryParamsToUpdate);
        }

        
    }
}
