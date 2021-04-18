using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace IdentityServer.Context
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var migrationsAssembly = typeof(AppDbContext).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<AppDbContext>(options =>
             options.UseSqlServer(connectionString,
                 sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)));

          
            
            return services;
        }
    }
}
