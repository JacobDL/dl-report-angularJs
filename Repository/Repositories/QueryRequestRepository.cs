using Repository.Database;
using Repository.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Repository.Repositories
{
    public class QueryRequestRepository : IQueryRequestRepository
    {
        private readonly IQueryRequestDb _queryRequestDb;
        public QueryRequestRepository(IQueryRequestDb queryRequestDb)
        {
            _queryRequestDb = queryRequestDb;
        }
        public DataTable GetSqlRequestToPage(QueryAndQueryParamsViewModel svm)
        {
            return _queryRequestDb.GetSqlRequestToPage(svm);
        }

        public MemoryStream GetSqlRequestToExcelFile(QueryAndQueryParamsViewModel svm)
        {
            return _queryRequestDb.GetSqlRequestToExcelFile(svm);
        }

    }
}
