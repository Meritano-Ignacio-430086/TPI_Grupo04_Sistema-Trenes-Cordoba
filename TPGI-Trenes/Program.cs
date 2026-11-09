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

            var app = builder.Build();

            builder.Services.AddScoped<ILineasRepository,LineasRepository>();
            builder.Services.AddScoped<ILineasServices,LineasServices>(); 
            builder.Services.AddScoped<QueryRepository>(); 

            builder.Services.AddDbContext<TrenesContext>(
                options => options.UseSqlServer(
                    //builder.Configuration.GetConnectionString("DefaultConnection")
                    builder.Configuration.GetConnectionString("Conneccion1")
                )
            );

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
