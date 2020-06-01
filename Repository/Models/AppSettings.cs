using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Models
{

    public class AppSettings
    {
        public string Secret { get; set; }
        public string AdministratorConnectionString { get; set; }
        public string UserConnectionString { get; set; }
    }
}
