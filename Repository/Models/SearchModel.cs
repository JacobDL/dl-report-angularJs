using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Models
{
    public class SearchModel
    {
        public Query SearchQuery { get; set; }
        public List<QueryParam> SearchQueryParams { get; set; }

    }
}
