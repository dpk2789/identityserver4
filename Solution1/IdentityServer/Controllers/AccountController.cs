using IdentityServer.Models;
using IdentityServer.Models.Request;
using IdentityServer.Models.Response;
using IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IdentityServer.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly IHostEnvironment _env;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AccountController(IIdentityService identityService, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,
            SignInManager<IdentityUser> signInManager, IWebHostEnvironment env,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _env = env;
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
        public async Task<IActionResult> Post([FromBody] UserRegisterRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthFailedResponse
                {
                    ErrorMessages = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                });
            }

            var user = new IdentityUser { UserName = request.email, Email = request.email };
            var createUser = await _userManager.CreateAsync(user, request.password);
            if (!createUser.Succeeded)
            {
                return BadRequest(new AuthFailedResponse
                {
                    ErrorMessages = createUser.Errors.Select(x => x.Description)
                });
            }
            if (createUser.Succeeded)
            {
                string returnUrl = null;
                returnUrl = returnUrl ?? Url.Content("~/");

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page("/",
                    pageHandler: null,
                    values: new { userId = user.Id, code = code, returnUrl = returnUrl },
                    protocol: Request.Scheme);

                string htmlString = System.IO.File.ReadAllText(Path.Combine(_env.ContentRootPath, "Templates\\Registration.html"));
                htmlString = htmlString.Replace("@@sitename@@", "Accounting On Web")
                     .Replace("@@emailid@@", request.email)
                     .Replace("@@accountcreated@@", DateTime.Now.ToString("dd-MM-yyyy"))
                      .Replace("@@activationlink@@", callbackUrl);

                await _emailSender.SendEmailAsync(request.email, "Confirm your email", htmlString);
                //$"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.")
                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    var role = _roleManager.FindByNameAsync("admin").Result;
                    await _userManager.AddToRoleAsync(user, role.Name);
                    return Ok(new AuthSuccessResponse
                    {
                        Success = true
                    });
                    // return RedirectToPage("RegisterConfirmation", new { email = request.email, returnUrl = returnUrl });
                }
                else
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Ok(new AuthSuccessResponse
                    {
                        Success = true
                    });
                    // return LocalRedirect(returnUrl);
                }
            }
            return BadRequest(new AuthResult
            {
                Success = false
            });
        }

        [TempData]
        public string StatusMessage { get; set; }

        [Route("api/account/registeruser")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
            return RedirectToAction("Index", "HomeABC");
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
