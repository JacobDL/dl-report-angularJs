using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Database.Query_and_QueryParam.Interfaces
{
    public interface ISelectQueryParam
    {
        public List<QueryParam> GetQueryParamsById(int id, bool needSqlList);
        public List<QueryParam> GetAllQueryParams();
    }
}
