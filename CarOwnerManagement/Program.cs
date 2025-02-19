using CarOwnerManagement.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CarOwnerManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers()
               .AddNewtonsoftJson(options => 
               {
                   options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                   options.SerializerSettings.Converters.Add(new StringEnumConverter());
               });

            builder.Services.AddDbContext<CarManagementDbContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("CarManagementDatabase")));
            
            builder.Services.AddSwaggerGen(builder =>
            {
                builder.SwaggerDoc("v1", new OpenApiInfo { Title = "CarOwnerManagement", Version = "v1" });
            });

            builder.Services.AddScoped<ICarRepository, CarRepository>();
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CarOwnerManagement v1"));
            }

            app.UseHttpsRedirection();
            app.MapControllers();
            
            app.Run();
        }
    }
}
