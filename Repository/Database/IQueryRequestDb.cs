using Repository.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Repository.Database
{
    public interface IQueryRequestDb
    {
        public DataTable GetSqlRequestToPage(QueryAndQueryParamsViewModel svm);
        public MemoryStream GetSqlRequestToExcelFile(QueryAndQueryParamsViewModel svm);
    }
}
