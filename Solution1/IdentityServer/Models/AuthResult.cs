using System.Collections.Generic;

namespace IdentityServer.Models
{
    public class AuthResult
    {
        public string Token { get; set; }
        public bool Success { get; set; }
        public IEnumerable<string> ErrorMessages { get; set; }
        public List<ErrorViewModel> ErrorViewModelEnu { get; set; }
    }
}
