
using API_Waylan_Origin.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using API_Waylan_Origin.Mapping;
using System.Text;
using System.Text.Json.Serialization; 



var builder = WebApplication.CreateBuilder(args);


// -------------------------------
// 1. Configurar conexión a MySQL
// -------------------------------

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//configuracion para que detecte la version de mysql
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseMySql(
    connectionString,
    ServerVersion.AutoDetect(connectionString)
    )
);

// -------------------------------
// 2.  Configurar JWT
// -------------------------------

var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// --------------------------------
// 3. Registro de servicios personalizados
// --------------------------------


// --------------------------------
// 4. Registro de AutoMapper
// --------------------------------

builder.Services.AddAutoMapper(config => config.AddProfile<MappingProfile>());


// --------------------------------
// 6. Swagger + Configuración JWT
// --------------------------------
// Swagger permite probar la API desde el navegador

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Waylan Origin API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization. Escribe 'Bearer ' seguido de tu token.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
