using Repository.Repositories.Query_and_QueryParam;
using Repository.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DreamLogisticsSqlSPA.ControllerLogic
{
    public class SearchControllerLogic
    {
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
