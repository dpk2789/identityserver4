using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ThirdParty.ViewModels
{
    public class Token
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
        public string ClientName { get; set; }
        public string ClientPassword { get; set; }
    }
}
