using ClosedXML.Excel;
using Repository.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace Repository.Database
{
    class QueryRequestDb
    {
        private readonly string connectionString = "Data Source=localhost;Initial Catalog=DreamLogisticsReport;Integrated Security=True";

        /// <summary>
        /// Gets the database result of the QueryString (Sql in Query) and the values added to the QueryParams
        /// On its own its used to display the result in a html view
        /// </summary>
        /// <param name="svm">
        /// the List<QueryParams> brings the ParameterCodes and the Values added by the user
        /// The Query brings the Sql-QueryString to know what to search for and which Table
        /// </param>
        /// <returns>A DataTable with the matching result to the parameters value</returns>
        internal DataTable GetSqlRequestToPage(QueryAndQueryParamsViewModel svm)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (var da = new SqlDataAdapter(svm.Query.Sql, connectionString))
                {

                    for (int i = 0; i < svm.QueryParams.Count; i++)
                    {
                        if (svm.QueryParams[i].Value != null)
                        {
                            da.SelectCommand.Parameters.AddWithValue($"{svm.QueryParams[i].ParameterCode}", svm.QueryParams[i].Value);
                        }
                        else
                        {
                            da.SelectCommand.Parameters.AddWithValue($"{svm.QueryParams[i].ParameterCode}", DBNull.Value);
                        }

                    }
                    da.Fill(dataTable);
                }
            }
            catch (Exception)
            {
               
            }
            return dataTable;
        }

        /// <summary>
        /// Gets the result of the Search in a DataTable from the function "GetSqlRequestToPage"
        /// then creates an memorystream to make it possible for the user to download the result to an excel file
        /// </summary>
        /// <param name="svm">
        /// the List<QueryParams> brings the ParameterCodes and the Values added by the user
        /// The Query brings the Sql-QueryString to know what to search for and which Table
        /// </param>
        /// <returns>A memorystream to be converted to a blob in AngularJs in the wwwroot/app/searchController</returns>
        internal MemoryStream GetSqlRequestToExcelFile(QueryAndQueryParamsViewModel svm)
        {
            DataTable dataTable = GetSqlRequestToPage(svm);

            MemoryStream ms = new MemoryStream();

            if (dataTable.Rows.Count != 0)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {

                    wb.Worksheets.Add(dataTable, "sql-request");

                    wb.SaveAs(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                }
            }
            return ms;
        }
    }
}
