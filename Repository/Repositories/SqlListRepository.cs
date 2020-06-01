using Repository.Database;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Repositories
{
    public class SqlListRepository : ISqlListRepository
    {
        private readonly ISqlListDb _sqlListDb;
        public SqlListRepository(ISqlListDb sqlListDb)
        {
            _sqlListDb = sqlListDb;
        }
        public List<SqlTable> GetSqlList(QueryParam queryParams)
        {
            return _sqlListDb.GetSqlList(queryParams);
        }
    }
}
