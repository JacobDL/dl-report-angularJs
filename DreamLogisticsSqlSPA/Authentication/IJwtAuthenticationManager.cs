using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DreamLogisticsSqlSPA
{
    public interface IJwtAuthenticationManager
    {
        string Authenticate(AuthenticateModel user);
    }
}
