using Aow.Domain;
using Microsoft.EntityFrameworkCore;

namespace Aow.Infrastructure
{
    public class AowDbContext : DbContext
    {
        public AowDbContext(DbContextOptions<AowDbContext> options)
           : base(options)
        {


        }
        public DbSet<Company> Companies { get; set; }
        public DbSet<UserCompanies> AppUserCompanies { get; set; }
        public DbSet<FinancialYear> FinancialYears { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
       
            base.OnModelCreating(builder);
        }
    }
}
