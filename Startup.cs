using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Serilog.Extensions.Logging.File;
using aspnetcore_rest_api_with_dapper.Data;
using aspnetcore_rest_api_with_dapper.Business;

namespace aspnetcore_rest_api_with_dapper
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
            services.AddTransient<IProductBusiness, ProductBusiness>();
            services.AddTransient<IProductRepository, ProductRepository>();
             // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "ASP.NET Core RESTful API", Version = "v1" });
            });
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            
            loggerFactory.AddFile("C:/@Logs/aspnetcore-rest-api-with-dapper-{Date}.txt");
 
            app.UseMiddleware(typeof(GlobalExceptionMiddleware));
            app.UseMvcWithDefaultRoute();

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ASP.NET Core RESTful API v1");
});
        }
    }
}
