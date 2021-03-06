using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;
using API.Middleware;
//using API.Extensions;
using Application.Activities;
using Application.Interfaces;
using Application.Utilities;
using FluentValidation.AspNetCore;
using Infrastructure.Photos;
using Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Persistence;

namespace API
{
    public class Startup
    {
        // public Startup(IConfiguration configuration)
        // {
        //     Configuration = configuration;
        // }
        // public IConfiguration Configuration { get; } //we're going to do this configuration injection differently - the commented out code is the default
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(opt =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
                //this will ensure that every endpoint in our API requires authentication (instead of adding [Authorize] attributes on Controller actions individually)
            })
                .AddFluentValidation(config =>
                {
                    //we're just telling it where our validators are coming from (just one of them - it will know where to look for the rest)
                    //we just need to specify a class that lives inside our Application project
                    config.RegisterValidatorsFromAssemblyContaining<Create>();
                });

            //to clean this up a bit, we could use an Extension method (API > Extensions > ApplicationServiceExtensions)
            //we would replace everything below here (not the services.AddControllers()!) with:
            //services.AddApplicationServices(_config);
            //we're doing it with IdentityServices below!

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
                //this is for API testing (like Postman) -> go to your <applicationUrl>/swagger/index.html
            });

            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlite(_config.GetConnectionString("DefaultConnection"));
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

            services.AddScoped<IUserAccessor, UserAccessor>(); //make sure we can use our Infrastructure UserAccessor and interface in the Application

            services.AddScoped<IPhotoAccessor, PhotoAccessor>();
            services.Configure<CloudinarySettings>(_config.GetSection("Cloudinary"));
            //this is how we bind our Cloudinary config in appsettings.json to our Infrastructure>CloudinarySettings class props

            services.AddIdentityServices(_config);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>(); //our own exception handling "page" for both dev and prod

            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            //app.UseHttpsRedirection();  //commented out because we decided not to use https -> launchSettings.json > API > applicationUrl (changed from "https://localhost:5001;http://localhost:5000" to just "http://localhost:5000")

            app.UseRouting();

            app.UseCors("CorsPolicy");

            //authentication MUST go before authorization!
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
