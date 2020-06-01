using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Database.Query_and_QueryParam.Interfaces
{
    public interface IDelete
    {
        public void DeleteQuery(int id);
        public void DeleteQueryParam(int id);
    }
}
