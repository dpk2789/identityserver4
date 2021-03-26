using IdentityModel.Client;
using System.Threading.Tasks;

namespace WebApp.MVC.Services
{
    public interface ITokenService
    {
        Task<TokenResponse> GetToken(string scope);
    }
}
