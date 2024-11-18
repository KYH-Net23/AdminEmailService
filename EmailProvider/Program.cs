using Azure.Identity;
using EmailProvider.EmailServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configure Azure Key Vault
var vaultUrl = new Uri(builder.Configuration["VaultUrl"]!);
builder.Configuration.AddAzureKeyVault(vaultUrl, new DefaultAzureCredential());

var connectionString = builder.Configuration["Rika-Email-Connection-String"]!;
var secretKey = builder.Configuration["Email-Service-Token-AccessKey"];

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("PaymentProvider", policy =>
         {
            policy.RequireClaim("PaymentProvider");
         })
    .AddPolicy("OrderProvider", policy =>
         {
             policy.RequireClaim("OrderProvider");
         })
    .AddPolicy("IdentityProvider", policy =>
        {
            policy.RequireClaim("IdentityProvider");
        });

// Add services

builder.Services.AddTransient<IdentityEmailService>();
builder.Services.AddTransient(_ => new OrderEmailService(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
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

// Configure authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = "https://www.rika.se/",
        ValidAudience = "https://www.rika.se/",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true
    };
});

// Build and configure the application
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API v1");
});

app.UseHttpsRedirection();
// app.UseMiddleware<PolicyMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
