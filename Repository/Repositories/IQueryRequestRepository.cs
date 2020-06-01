using Repository.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Repository.Repositories
{
    public interface IQueryRequestRepository
    {
        public DataTable GetSqlRequestToPage(QueryAndQueryParamsViewModel svm);

        public MemoryStream GetSqlRequestToExcelFile(QueryAndQueryParamsViewModel svm);
    }
}
