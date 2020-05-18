using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Repository.Models
{
    public class QueryParam
    {
        public int Id { get; set; }
        public int QueryId { get; set; }
        [Display(Name = "Parameternamn")]
        public string Name { get; set; }
        public int TypeId { get; set; }
        public string ParameterCode { get; set; }
        public string TableName { get; set; }
        public string DisplayColumn { get; set; }
        public string KeyColumn { get; set; }
        public bool ParamOptional { get; set; }


        public string Value { get; set; }
        public List<SqlTable> SqlLists { get; set; }

        public QueryParam()
        {

        }
        public QueryParam(string name, int typeId, string parameterCode, bool paramOptional)
        {
            Name = name;
            TypeId = typeId;
            ParameterCode = parameterCode;
            ParamOptional = paramOptional;
        }
    }
}
