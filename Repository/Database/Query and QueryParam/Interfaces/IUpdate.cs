﻿using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Database.Query_and_QueryParam.Interfaces
{
    public interface IUpdate
    {
        public void UpdateQuery(Query query, List<QueryParam> queryParams);
    }
}
