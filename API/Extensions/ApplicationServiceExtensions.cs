using Application.Activities;
using Application.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Persistence;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        //this is simply to clean up the Startup ConfigureServices method and make it easier to read
        // -- totally optional! (you could just leave everything in there as is too)
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
                //this is for API testing (like Postman) -> go to your <applicationUrl>/swagger/index.html
            });

            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
                //this connection string is configured in appsettings.Development.json:
                //"ConnectionStrings": { "DefaultConnection": "Data source=reactivities.db" }
            });

            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:3000");
                });
            }); // this is so we can tell Cors we know it's ok to pass data back and forth in different localhost ports
            //once we get to production, the client-app and API will be on the same domain, so we won't need it

            services.AddMediatR(typeof(List.Handler).Assembly); //tells our app to use MediatR, and the rest tells MediatR where to find our handlers
            services.AddAutoMapper(typeof(MappingProfiles).Assembly); //a utility we can use to map properties from one object to another object

            return services;
        }
    }
}