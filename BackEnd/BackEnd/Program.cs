using BackEnd.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer; //para autenticar con JWT
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens; //para validar el JWT
using System.Text; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Configuración del JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //cada que una peticion es mandada busca un JWT, lo descifra con el key y si es valido lee el usuario
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    //Si no tiene autorizacion no le permite ver nada: 401
}).AddJwtBearer(options => //configura el JWT
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true, //valida que no haya expirado
        ValidateIssuerSigningKey = true, //valida que el token haya sido firmado con el key correcto
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"], //lo busca en appsettings.json, dentro de Jwt
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!))
        //Uso ! porque sabemos que el valor no será null, ya que lo he puesto en appsettings.json
    };
});


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//CORS de prueba
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

//Añadiendo context
builder.Services.AddDbContext<WaybackContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
//-----------

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
    
app.UseHttpsRedirection();

//CORS de prueba
app.UseCors("AllowFrontend");

app.UseAuthentication(); //antes de la autorizacion, para que pueda autenticar al usuario y luego verificar sus permisos
app.UseAuthorization();

app.MapControllers();

app.Run();
