using Aow.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Reflection;

namespace WebApi1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInfrastructure(Configuration);
            services.AddControllers();


            services.AddAuthentication("Bearer")
               .AddIdentityServerAuthentication("Bearer", options =>
               {
                   options.ApiName = "WebApi1";
                   options.Authority = "https://localhost:44311";
               });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ToDo API",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Shayne Boyer",
                        Email = string.Empty,
                        Url = new Uri("https://twitter.com/spboyer"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("PublicSecure", policy => policy.RequireClaim("client_id", "secret_client_id"));
            //    options.AddPolicy("UserSecure", policy => policy.RequireClaim("roleType", "CanReaddata"));
            //    options.AddPolicy("AdminSecure", policy => policy.RequireClaim("roleType", "CanUpdatedata"));
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });           

            app.UseSwagger(options =>
            {
                options.RouteTemplate = "/swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger Demo API");
                options.RoutePrefix = string.Empty;
            });
        }
    }
}
