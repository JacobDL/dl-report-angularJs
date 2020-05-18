using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.ViewModels
{
    public class QueryAndQueryParamsViewModel
    {
        public Query Query { get; set; }
        public List<QueryParam> QueryParams { get; set; }
        public List<int> QueryParamsToDelete { get; set; }
    }
}
