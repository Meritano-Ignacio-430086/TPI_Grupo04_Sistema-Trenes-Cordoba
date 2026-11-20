using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

            var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>() 
                    ?? throw new InvalidOperationException("No esta configurado el JwtSettings"); 
            builder.Services.AddSingleton(jwtSettings);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => 
            {
                options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
            };
        });
            builder.Services.AddAuthorization();

            builder.Services.AddScoped<ILineasRepository,LineasRepository>();
            builder.Services.AddScoped<ILineasServices,LineasServices>(); 
            builder.Services.AddScoped<QueryRepository>(); 
            builder.Services.AddScoped<ITicketsRepository,TicketsRepository>(); 
            builder.Services.AddScoped<ITicketsServices,TicketsServices>(); 
            builder.Services.AddScoped<IUserRepository,UserRepository>(); 
            builder.Services.AddScoped<IAuthService,AuthService>();
            builder.Services.AddScoped<IMantenimientosRepository, MantenimientosRepository>();
            builder.Services.AddScoped<IMantenimientosServices, MantenimientosServices>();

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


            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
