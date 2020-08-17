using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace AspNetCoreUseSwagger
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
            services.AddControllers();

            // Register the Swagger generator, defining one or more Swagger documents
            // https://docs.microsoft.com/zh-cn/aspnet/core/tutorials/getting-started-with-swashbuckle
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                //c.EnableAnnotations();

                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "AspNetCoreUseSwagger.xml"), true);
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Domain.xml"), true);

                //c.OperationFilter<AddAuthTokenOperationFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }


    public class AddAuthTokenOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            context.ApiDescription.TryGetMethodInfo(out MethodInfo methodInfo);
            var allowAnonymousAttribute = methodInfo.GetCustomAttribute<AllowAnonymousAttribute>(false);

            if (allowAnonymousAttribute == null)
            {
                operation.Parameters.Add(new OpenApiParameter()
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Description = "Authorization认证信息",
                    Required = true
                });
            }
        }
    }
}
