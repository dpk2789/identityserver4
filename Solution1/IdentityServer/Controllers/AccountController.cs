using IdentityServer.Models.Request;
using IdentityServer.Models.Response;
using IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IdentityServer.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public AccountController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        //[HttpGet]
        //[Route("api/getusers")]
        //public async Task<IReadOnlyList<IdentityUser>> Get()
        //{
        //    return await _userManager.Users.ToListAsync();
        //}

        // GET api/<AccountController>/5
        [HttpGet("{id}")]
        [Route("api/getusersbyid")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        [Route("api/account/registeruser")]
        [AllowAnonymous]
        public async Task<IActionResult> Postabc([FromBody] UserRegisterRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthFailedResponse
                {
                    ErrorMessages = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                });
            }

            var authResult = await _identityService.RegisterAsync(request.email, request.email);
            if (!authResult.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    ErrorMessages = authResult.ErrorMessages
                });
            }
            return Ok(new AuthSuccessResponse
            {
                Token = authResult.Token
            });
        }


        // PUT api/<AccountController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AccountController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
