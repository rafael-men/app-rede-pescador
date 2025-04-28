using Microsoft.EntityFrameworkCore;
using System;
using Npgsql;
using Serilog;
using rede_pescador_api.Data;
using Microsoft.OpenApi.Models;
using rede_pescador_api.Repository;
using rede_pescador_api.Repositories;
using rede_pescador_api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using rede_pescador_api.Orders;


var builder = WebApplication.CreateBuilder(args);


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddSingleton<Serilog.ILogger>(Log.Logger);
var evolveLogger = Log.ForContext("SourceContext", "Evolve");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API da aplicação web rede pescador com .NET",
        Version = "v1",
        Description = "Backend do sistema de venda de peixes da rede pescador com C#."
    });

   
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT no campo abaixo usando o esquema Bearer. Exemplo: Bearer {seu_token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var key = Encoding.ASCII.GetBytes("8d99dX1fZPqfD56Tkcj3pZdTdzdfsdfsdfdffwefsdcsrggwsfdsa");

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = "RedePescadorAPI",
        ValidateAudience = true,
        ValidAudience = "RedePescadorClient",
    };
});


builder.Services.AddAuthorization(options =>
{
 
    options.AddPolicy("Consumidor", policy =>
        policy.RequireRole("CONSUMIDOR"));

 
    options.AddPolicy("Pescador", policy =>
        policy.RequireRole("PESCADOR"));

});

//builder.Services.AddAuthentication(options =>
//{
//   options.DefaultScheme = "Cookies";
//    options.DefaultChallengeScheme = "Google";
//})
//.AddCookie("Cookies")
//.AddGoogle("Google", options =>
//{
  // options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
  // options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
  //  options.CallbackPath = "/google-callback"; 
//});


builder.Services.AddScoped<IProductService, ProductServiceImpl>();
builder.Services.AddScoped<IProductRepository, ProductRepositoryImpl>();
builder.Services.AddScoped<IUserRepository, UserRepositoryImpl>();
builder.Services.AddScoped<IUserService, UserServiceImpl>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenServiceImpl>();
builder.Services.AddScoped<IOrderRepository,OrderRepositoryImpl>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<IRatingRepository,RatingRepositoryImpl>();
builder.Services.AddScoped<RatingService>();
var app = builder.Build();
DatabaseMigration.MigrateDatabase(app.Configuration, evolveLogger);
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Gerenciamento");
        c.RoutePrefix = string.Empty; 
    });
}
app.UseCors("AllowAll");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization(); 
app.MapControllers();
app.Run();


public static class DatabaseMigration
{
    public static void MigrateDatabase(IConfiguration configuration, Serilog.ILogger logger)
    {
        try
        {
            logger.Information("Iniciando as migrações de banco de dados com Evolve.");
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                logger.Error("A string de conexão 'DefaultConnection' não foi configurada.");
                return;
            }

            using (var connection = new NpgsqlConnection(connectionString))
            {
                var evolve = new Evolve.Evolve(connection, msg => logger.Information(msg))
                {
                    Locations = new[] { "db" },
                    IsEraseDisabled = true,
                };

                evolve.Migrate();

                logger.Information("Migrações de banco de dados concluídas com sucesso.");
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Ocorreu um erro durante as migrações de banco de dados.");
            throw;
        }
    }
}
