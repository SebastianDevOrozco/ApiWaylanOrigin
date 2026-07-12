
using API_Waylan_Origin.Data;
using API_Waylan_Origin.Interfaces.Categoria;
using API_Waylan_Origin.Interfaces.Notas;
using API_Waylan_Origin.Interfaces.Pedidos;
using API_Waylan_Origin.Interfaces.Producto;
using API_Waylan_Origin.Interfaces.Usuarios;
using API_Waylan_Origin.Interfaces.Wompi;
using API_Waylan_Origin.Mapping;
using API_Waylan_Origin.Services.Autenticacion;
using API_Waylan_Origin.Services.Categorias;
using API_Waylan_Origin.Services.Notas;
using API_Waylan_Origin.Services.Pedidos;
using API_Waylan_Origin.Services.Productos;
using API_Waylan_Origin.Services.Usuarios;
using API_Waylan_Origin.Services.Wompi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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

builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<INotaService, NotaService>();
builder.Services.AddScoped<IPagosService, PagosService>();

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

// -------------------------------
// 7. CORS
// -------------------------------

// Definimos un nombre para nuestra política
var ReglasCors = "ReglasCorsFrontend";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: ReglasCors, policy =>
    {
        policy.AllowAnyOrigin() // cuando este en la nube debo cambiarlo por el dominio real de la pagina web
              .AllowAnyHeader()   // Permite cualquier tipo de encabezado (como los tokens de seguridad)
              .AllowAnyMethod();  // Permite cualquier método (GET, POST, PUT, DELETE)
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

app.UseMiddleware<API_Waylan_Origin.Middleware.ExceptionMiddleware>();

app.UseHttpsRedirection();

//permisos para habilitar los archivos estaticos
app.UseStaticFiles();

app.UseCors(ReglasCors);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
