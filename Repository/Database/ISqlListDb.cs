using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Database
{
    public interface ISqlListDb
    {
        public List<SqlTable> GetSqlList(QueryParam queryParam);
    }
}
