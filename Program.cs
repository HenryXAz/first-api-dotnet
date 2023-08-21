using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using proyectoToken.Models;
using proyectoToken.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//context
builder.Services.AddDbContext<ApiDotnetContext>(options => {
  var connection = builder.Configuration.GetConnectionString("cadenaSql");
  options.UseMySql(connection, ServerVersion.AutoDetect(connection));
});

//token service
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
var key = builder.Configuration.GetValue<String>("JwtSettings:key");
var keyBytes = Encoding.ASCII.GetBytes(key);

builder.Services.AddAuthentication(config => {
  config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config => {
  config.RequireHttpsMetadata = false;
  config.SaveToken = true;
  config.TokenValidationParameters = new TokenValidationParameters{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
    ValidateIssuer = false,
    ValidateAudience = false,
    ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero
  };

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// https 
// app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
