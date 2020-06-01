using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Database.Query_and_QueryParam.Interfaces
{
    public interface IInsert
    {
        public void InsertQuery(Query query, List<QueryParam> queryParams);
        public void InsertQueryParam(List<QueryParam> queryParams, int queryId);
    }
}
