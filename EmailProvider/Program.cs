using Azure.Identity;
using Azure.Messaging.ServiceBus;
using EmailProvider.EmailServices;
using EmailProvider.EmailServices.EmailQueue;
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
var azureServiceBusConnection = builder.Configuration["rika-service-bus"];



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
builder.Services.AddSingleton(new ServiceBusClient(azureServiceBusConnection));
builder.Services.AddSingleton(_ => new OrderEmailService(connectionString));
builder.Services.AddSingleton(_ => new WelcomeEmailService(connectionString));
builder.Services.AddSingleton(_ => new VerificationEmailService(connectionString));
builder.Services.AddSingleton(_ => new ResetPasswordService(connectionString));
builder.Services.AddSingleton<EmailQueueService>(x =>
{
    var client = x.GetRequiredService<ServiceBusClient>();
    var queueName = builder.Configuration["QueueName"];
    return new EmailQueueService(client, queueName!);
});
builder.Services.AddHostedService<EmailProcessingService>(x =>
{
    var serviceBusClient = x.GetRequiredService<ServiceBusClient>();
    var queueName = builder.Configuration["QueueName"];
    var orderEmailService = x.GetRequiredService<OrderEmailService>();
    var welcomeEmailService = x.GetRequiredService<WelcomeEmailService>();
    var verificationEmailService = x.GetRequiredService<VerificationEmailService>();
    var resetEmailService = x.GetRequiredService<ResetPasswordService>();
    return new EmailProcessingService(serviceBusClient, orderEmailService, queueName!, welcomeEmailService, verificationEmailService, resetEmailService);
});
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
