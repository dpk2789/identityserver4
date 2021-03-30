using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models.Request
{
    public class UserRegisterRequest
    {
        [EmailAddress]
        public string email { get; set; }
        public string password { get; set; }
    }
}
