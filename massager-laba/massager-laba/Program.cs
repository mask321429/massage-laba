using Amazon.S3;
using massager_laba.Data;
using massager_laba.Interface;
using massager_laba.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddDbContext<DBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Конфигурация клиента Amazon S3
builder.Services.AddAWSService<IAmazonS3>();

// Регистрация сервиса
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = TokenConfigurations.Issuer,
            ValidateAudience = true,
            ValidAudience = TokenConfigurations.Audience,
            ValidateLifetime = true,
            IssuerSigningKey = TokenConfigurations.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
        };
    });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<DBContext>();
        dbContext.Database.Migrate();
        if (!dbContext.Database.GetPendingMigrations().Any())
        {
            dbContext.Database.EnsureCreated(); 
            Console.WriteLine("Automatically applied migration.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error occurred while migrating: {ex.Message}");
    }
}

app.UseCors(); // Вызовите UseCors

app.UseHttpsRedirection();

app.UseAuthentication(); // Добавьте эту строку
app.UseAuthorization();

app.MapControllers();

app.Run();
