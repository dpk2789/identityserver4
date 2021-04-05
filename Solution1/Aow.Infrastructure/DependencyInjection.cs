using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aow.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContext<AowDbContext>(options =>
            //    options.UseSqlServer(
            //        configuration.GetConnectionString("DefaultConnection"),
            //        b => b.MigrationsAssembly(typeof(AowDbContext).Assembly.FullName)));

            services.AddDbContextPool<AowDbContext>(options =>
                options.UseMySQL(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(AowDbContext).Assembly.FullName)));

            return services;
        }
    }
}
