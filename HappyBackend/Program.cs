using AspNetCoreRateLimit;
using HappyBackEnd.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace HappyBackend
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            // Add services to the container.
            builder.Services.AddControllers();

            // Add memory cache 
            builder.Services.AddMemoryCache();

            // Load configuration from appsettings
            builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
            builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));

            // Add in-memory rate limiting stores
            builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();

            // Add rate limit configuration
            builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            // Add IP rate limiting middleware
            builder.Services.AddInMemoryRateLimiting();
            


            // Add Swagger services
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "HappyTeam API",
                    Description = "API for managing data",
                    
                    Contact = new OpenApiContact
                    {
                        Name = "Oskar Kaca³a",
                        Email = "OskarKacala43@gmail.com",
                    }

                });
            });

            var app = builder.Build();

            app.UseCors("AllowAll");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
                // specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Happy API V1");
                    c.RoutePrefix = string.Empty; 
                });
            }

            app.UseHttpsRedirection();

            app.UseIpRateLimiting(); 

            app.UseAuthorization();

            app.MapControllers();

            HappyTeslaRepository.LoadData();

            app.Run();
        }
    }
}
