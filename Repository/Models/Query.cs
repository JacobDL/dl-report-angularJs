using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Repository.Models
{
    public class Query
    {
        public int Id { get; set; }
        public string Sql { get; set; }
        [Display(Name = "Namn")]
        public string GroupName { get; set; }

        public Query()
        {

        }
        public Query(string sql, string groupName)
        {
            Sql = sql;
            GroupName = groupName;
        }
    }

}
