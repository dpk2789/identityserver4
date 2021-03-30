using IdentityServer.Models.Request;
using IdentityServer.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IdentityServer.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        [Route("api/getusers")]
        public async Task<IReadOnlyList<IdentityUser>> Get()
        {
            return await _userManager.Users.ToListAsync();
        }

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
        public async Task<IActionResult> Post([FromBody] UserRegisterRequest request, CancellationToken cancellationToken)
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
