using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Database.Query_and_QueryParam.Interfaces
{
    public interface ISelectQuery
    {
        public List<Query> GetQueries();
        public Query GetQueryById(int id);
    }
}
