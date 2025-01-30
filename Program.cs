using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using school_project.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 📌 **Swagger API Dökümantasyonu**
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 📌 **Veritabanı Bağlantısı**
builder.Services.AddDbContext<SchoolDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 📌 **JWT Authentication (Kimlik Doğrulama)**
var jwtKey = "your-very-long-and-secure-secret-key-12345";
var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

// 📌 **Yetkilendirme Servisini EKLE**
builder.Services.AddAuthorization();

// 📌 **CORS Ayarları (Frontend Bağlantısı İçin)**
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 📌 **⚠️ Eksik Olan AddControllers() Metodu Eklendi!**
builder.Services.AddControllers(); // **HATA BURADAN KAYNAKLANIYORDU!**

var app = builder.Build();

// 📌 **Development Moduysa Swagger Aç**
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 📌 **Middleware (Ara Katman Yazılımlarını Bağla)**
app.UseCors("AllowAllOrigins"); // CORS Kullanımı
app.UseAuthentication(); // **Kimlik Doğrulama**
app.UseAuthorization();  // **Yetkilendirme**

app.MapControllers(); // **Controller'ları Kullan!**

// 📌 **Backend Port Ayarları**
app.Urls.Add("http://0.0.0.0:5000");  // Tüm Ağlara Aç
app.Urls.Add("http://localhost:5000"); // Lokal Makineye Aç

app.Run();
