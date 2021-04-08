using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace WebApp.ThirdParty.Helpers
{
    public class UserSession 
    {
        //public string Username
        //{
        //    get {
        //        if (HttpContext. is ClaimsIdentity identity)
        //        {
        //            var principal = new ClaimsPrincipal(identity);
        //            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                   
        //        }
        //        return userId;
        //    }
        //}

        //public string BearerToken
        //{
        //    get { return ((ClaimsPrincipal)HttpContext.Current.User).FindFirst("AcessToken").Value; }
        //}

    }
}
