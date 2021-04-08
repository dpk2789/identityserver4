using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ThirdParty.Helpers
{
    public interface IUserSession
    {
        string Username { get; }
        string BearerToken { get; }
    }
}
