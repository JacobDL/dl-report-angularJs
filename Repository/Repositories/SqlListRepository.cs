using Repository.Database;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Repositories
{
    public class SqlListRepository
    {
        public static List<SqlTable> GetSqlList(QueryParam queryParams)
        {
            SqlListDb db = new SqlListDb();
            return db.GetSqlList(queryParams);
        }
    }
}
