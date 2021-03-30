using IdentityServer.Models;
using System.Threading.Tasks;

namespace IdentityServer.Services
{
    interface IIdentityService
    {
        Task<AuthResult> RegisterAsync(string Email, string Password);
       // Task<AuthResult> LoginAsync(string Email, string Password);
    }
}
