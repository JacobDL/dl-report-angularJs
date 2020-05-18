using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.ViewModels
{
    public class SqlResultViewModel
    {
        public List<string> ColumnNames { get; set; }
        public List<List<string>> RowValues { get; set; }
        public string Message { get; set; }
    }
}
