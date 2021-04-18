//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Threading.Tasks;

//namespace IdentityServer.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class UserController : ControllerBase
//    {        
//        // [HttpGet("{id}")]
//        public async Task<ActionResult<IdentityUser>> GetCompany(Guid id)
//        {
//            var company = await _context.Companies.FindAsync(id);

//            if (company == null)
//            {
//                return NotFound();
//            }

//            return company;
//        }
//    }
//}
