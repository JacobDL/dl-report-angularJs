using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Repositories
{
    public interface ISqlListRepository
    {
        public List<SqlTable> GetSqlList(QueryParam queryParams);
    }
}
