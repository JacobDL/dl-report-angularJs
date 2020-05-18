using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.ViewModels
{
    public class SearchViewModel
    {
        public Query Query { get; set; }
        public List<QueryParam> QueryParams { get; set; }
    }
}
