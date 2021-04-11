using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aow.Domain;
using Aow.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using WebApi1.Contracts.V1;

namespace WebApi1.Controllers
{
    //  [Route("api/[controller]")]
    [ApiController]
    [Authorize(policy: "UserSecure")]
    public class CompaniesController : ControllerBase
    {
        private readonly AowDbContext _context;

        public CompaniesController(AowDbContext context)
        {
            _context = context;
        }

        // GET: api/Companies
        [HttpGet(ApiRoutes.Companies.GetAll)]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
        {
            return await _context.Companies.ToListAsync();
        }

        [HttpGet(ApiRoutes.Companies.Get)]
        public async Task<ActionResult<Company>> GetCompany(Guid id)
        {
            var company = await _context.Companies.FindAsync(id);

            if (company == null)
            {
                return NotFound();
            }

            return company;
        }

        
        //[HttpPut("{id}")]
        [HttpPut(ApiRoutes.Companies.Update)]
        public async Task<IActionResult> PutCompany(Guid id, Company company)
        {
            if (id != company.Id)
            {
                return BadRequest();
            }

            _context.Entry(company).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        
        [HttpPost(ApiRoutes.Companies.Create)]
        public async Task<ActionResult<Company>> PostCompany(Company company)
        {
            var user = HttpContext.User.Identity;
            var request = HttpContext.Request;
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCompany", new { id = company.Id }, company);
        }
               
        //[HttpDelete("{id}")]
        [HttpDelete(ApiRoutes.Companies.Delete)]
        public async Task<ActionResult<Company>> DeleteCompany(Guid id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            return company;
        }

        private bool CompanyExists(Guid id)
        {
            return _context.Companies.Any(e => e.Id == id);
        }
    }
}
