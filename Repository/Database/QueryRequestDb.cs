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

        internal DataTable GetSqlRequestToPage(QueryAndQueryParamsViewModel svm)
        {
            DataTable dataTable = new DataTable();

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
            return dataTable;
        }

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
