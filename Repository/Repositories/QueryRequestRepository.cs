using Repository.Database;
using Repository.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Repository.Repositories
{
    public class QueryRequestRepository
    {
        public static DataTable GetSqlRequestToPage(QueryAndQueryParamsViewModel svm)
        {
            QueryRequestDb db = new QueryRequestDb();
            return db.GetSqlRequestToPage(svm);
        }

        public static MemoryStream GetSqlRequestToExcelFile(QueryAndQueryParamsViewModel svm)
        {
            QueryRequestDb db = new QueryRequestDb();
            return db.GetSqlRequestToExcelFile(svm);
        }

    }
}
