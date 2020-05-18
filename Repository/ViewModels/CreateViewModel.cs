using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.ViewModels
{
    public class CreateViewModel
    {
        public string GroupName { get; set; }
        public string Sql { get; set; }
        public List<Row> Rows { get; set; }

    }

    public class Row
    {
        public string Name { get; set; }
        public int TypeId { get; set; }
        public string ParameterCode { get; set; }
        public string TableName { get; set; }
        public string DisplayColumn { get; set; }
        public string KeyColumn { get; set; }
        public bool ParamOptional { get; set; }
    }
}
