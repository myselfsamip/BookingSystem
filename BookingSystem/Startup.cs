using BookingSystem.Application.IServices;
using BookingSystem.Application.Services;
using BookingSystem.Core.Interfaces;
using BookingSystem.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;

namespace BookingSystem
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IBookingRepository, BookingRepository>();

            // Add Swagger services
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {

                    Title = "Booking.API",
                    Version = "v1",
                    Description = "<h3>Booking platform web api service </h3>",
                    Contact = new OpenApiContact
                    {
                        Name = "API Developer",
                        Email = "myselfsamip@gmail.com"
                    }

                });
            });
            services.AddControllers();

           
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Booking System");

            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
