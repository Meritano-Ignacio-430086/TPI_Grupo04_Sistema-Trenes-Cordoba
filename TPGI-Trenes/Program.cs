using Microsoft.EntityFrameworkCore;
using TPGI_Trenes.Negocio;
using TPGI_Trenes.Repository;
using TPGI_Trenes.Repository.Interfaces;
using TPGI_Trenes.Services;
using TPGI_Trenes.Services.Interfaces;

namespace TPGI_Trenes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            builder.Services.AddScoped<ILineasRepository,LineasRepository>();
            builder.Services.AddScoped<ILineasServices,LineasServices>(); 
            builder.Services.AddScoped<QueryRepository>(); 
            builder.Services.AddScoped<ITicketsRepository,TicketsRepository>(); 
            builder.Services.AddScoped<ITicketsServices,TicketsServices>(); 

            builder.Services.AddDbContext<TrenesContext>(
                options => options.UseSqlServer(
                //builder.Configuration.GetConnectionString("DefaultConnection")
                //builder.Configuration.GetConnectionString("Conneccion1")
                builder.Configuration.GetConnectionString("Caccioppoli")
                )
            );

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            app.UseCors();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
