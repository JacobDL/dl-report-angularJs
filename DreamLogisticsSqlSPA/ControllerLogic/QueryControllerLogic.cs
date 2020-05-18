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

        //Gets a Query by QueryId and the QueryParams that belongs to it
        //Also adds the values from the users search information to the "QueryAndQueryParamsViewModel"
        internal static QueryAndQueryParamsViewModel GetQueryResult(SearchParameters parameters)
        {
            bool needSqlList = true;
            QueryAndQueryParamsViewModel result = new QueryAndQueryParamsViewModel();
            result.Query = QueryRepository.GetQueryById(parameters.QueryId);
            result.QueryParams = QueryRepository.GetQueryParamsByQueryId(parameters.QueryId, needSqlList);
            for (int i = 0; i < result.QueryParams.Count; i++)
            {
                if (parameters.Parameters[i].Value != "null")
                {
                    result.QueryParams[i].Value = parameters.Parameters[i].Value;
                }
               
                
            }
            return result;
        }

        //Converts the DataTable into readable variables and adds it to the "SqlResultViewModel"
        internal static SqlResultViewModel GetResultViewModel(DataTable sqlResults)
        {
            SqlResultViewModel resultViewModel = new SqlResultViewModel();

            resultViewModel.ColumnNames = GetColumnNamesFromDataTable(sqlResults);
            resultViewModel.RowValues = GetListRowResultFromDataTable(sqlResults);

            if (resultViewModel.RowValues.Count == 0)
            {
                resultViewModel.Message = "Det fanns inga matchande resultat!";
            }
            else
            {
                resultViewModel.Message = "Nerladdning slutförd!";
            }

            return resultViewModel;
        }

        //Convert the information from the "CreateViewModel" to an List<QueryParam> Object

            //The reason being to be able to use the already existing function to insert Query and QueryParams to the database
        internal static List<QueryParam> ConvertQueryParams(CreateViewModel formData)
        {
            List<QueryParam> queryparams = new List<QueryParam>();
            foreach (var row in formData.Rows)
            {
                QueryParam queryParam = new QueryParam(row.Name, row.TypeId, row.ParameterCode, row.ParamOptional);
                if (row.TableName.Length != 0)
                {
                    queryParam.TableName = row.TableName;
                    queryParam.DisplayColumn = row.DisplayColumn;
                    queryParam.KeyColumn = row.KeyColumn;
                }
                queryparams.Add(queryParam);
            }
            return queryparams;
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

        //Converts column names to a List<string>
        private static List<string> GetColumnNamesFromDataTable(DataTable sqlResults)
        {
            List<string> columnNames = new List<string>();

            foreach (var column in sqlResults.Columns)
            {
                columnNames.Add(column.ToString());
            }

            return columnNames;
        }

        //Converts Row results to a List<string>
        private static List<List<string>> GetListRowResultFromDataTable(DataTable dataResult)
        {
            List<List<string>> result = new List<List<string>>();

            foreach (DataRow rows in dataResult.Rows)
            {
                List<string> row = new List<string>();

                foreach (DataColumn col in dataResult.Columns)
                {
                    row.Add(rows[col].ToString());
                }
                result.Add(row);
            }

            return result;
        }
    }
}
