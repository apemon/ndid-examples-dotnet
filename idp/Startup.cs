using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using idp.Services;
using idp.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace idp
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
            services.AddMvc();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "NDID IDP POC APP", Version = "v1" });
            });

            // Enable CORS
            services.AddCors();

            // Add application services via dependencies injection
            services.AddTransient<IConfigurationService, EnvironmentConfigurationService>();
            services.AddTransient<INDIDService, NDIDService>();
            services.AddTransient<IDPKIService, FileBasedDPKIServicec>();
            services.AddSingleton<IPersistanceStorageService, LiteDBStorageService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "NDID IDP POC APP");
            });

            // Enable CORS
            app.UseCors(builder =>
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            // Exception handling
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            // Request/Response logging
            app.UseMiddleware<RequestLoggingMiddleware>();

            app.UseMvc();
        }
    }
}
