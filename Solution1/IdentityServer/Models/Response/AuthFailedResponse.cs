using System.Collections.Generic;

namespace IdentityServer.Models.Response
{
    public class AuthFailedResponse
    {
        public IEnumerable< string> ErrorMessages { get; set; }
    }
}
