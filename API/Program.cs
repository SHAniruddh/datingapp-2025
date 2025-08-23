using Microsoft.EntityFrameworkCore;
using API.Entity; // ✅ This matches your AppUser class
using API.Data;
using System.Text;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddCors();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    var tokenKey = builder.Configuration["TokenKey"]
    ?? throw new Exception("Token key not found - Program.cs");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
        ValidateIssuer = false,
        ValidateAudience=false
    };
});
var app = builder.Build();

// ✅ SEED DATA
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();

    if (!context.Users.Any())
    {
        var dummyHash = Encoding.UTF8.GetBytes("hash");
        var dummySalt = Encoding.UTF8.GetBytes("salt");

        context.Users.AddRange(
            new AppUser 
            { 
                DisplayName = "Bob", 
                Email = "bob@test.com",
                PasswordHash = dummyHash,
                PasswordSalt = dummySalt
            },
            new AppUser 
            { 
                DisplayName = "Tom", 
                Email = "tom@test.com",
                PasswordHash = dummyHash,
                PasswordSalt = dummySalt
            },
            new AppUser 
            { 
                DisplayName = "Jane", 
                Email = "jane@test.com",
                PasswordHash = dummyHash,
                PasswordSalt = dummySalt
            }
        );
        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline
app.UseRouting();
app.UseCors(x =>
    x.AllowAnyHeader()
     .AllowAnyMethod()
     .WithOrigins("http://localhost:4200", "https://localhost:4200"));
app.UseAuthentication();
app.UseAuthentication();
app.MapControllers();
app.Run();

