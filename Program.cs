using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using school_project.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Swagger ve API endpoint'lerini ekle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Veritaban� Ba�lant�s�
builder.Services.AddDbContext<SchoolDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// JWT Secret Key
var jwtKey = "your-very-long-and-secure-secret-key-12345"; 
var key = Encoding.ASCII.GetBytes(jwtKey);

// JWT kimlik do�rulama
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

// CORS Ayarlar�
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5000", "https://localhost:44307")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Controller'lar� ve Authentication ekle
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware Ayarlar�
app.UseCors("AllowFrontend"); // CORS Kullan�m�
app.UseAuthentication(); // JWT Kimlik Do�rulama
app.UseAuthorization(); // Yetkilendirme

// Controller'lar� ba�la
app.MapControllers();

app.Run();
