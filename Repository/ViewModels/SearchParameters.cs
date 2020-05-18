using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.ViewModels
{
    public class SearchParameters
    {
        public int QueryId { get; set; }
        public  List<Parameter> Parameters { get; set; }
    }

    public class Parameter
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
}
