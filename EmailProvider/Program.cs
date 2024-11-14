using APIWeaver;
using Azure.Identity;
using EmailProvider.Extensions;
using EmailProvider.Middleware;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi(options =>
{
    options.AddSecurityScheme("Bearer", scheme =>
    {
        scheme.Scheme = "Basic";
        scheme.BearerFormat = "JWT";
        scheme.In = ParameterLocation.Header;
        scheme.Name = "Authorization";
        scheme.Description = "Bearer token";
        scheme.Type = SecuritySchemeType.Http;
        scheme.Flows = new OpenApiOAuthFlows
        {
            ClientCredentials = new OpenApiOAuthFlow
            {
                TokenUrl = new Uri("https://localhost:5001/oauth2/token")
            }
        };
    });
    options.AddAuthResponse();
});

var connectionString = builder.Configuration["Rika-Email-Connection-String"]!;
var secretKey = builder.Configuration["Email-Service-Token-AccessKey"];
var vaultUrl = new Uri(builder.Configuration["VaultUrl"]!);

builder.Configuration.AddAzureKeyVault(vaultUrl, new DefaultAzureCredential());

builder.Services.AddControllers();

builder.Services.AddAuthenticationExtension(builder);

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("IdentityPolicy", policy => policy.RequireClaim("Identity"))
    .AddPolicy("OrderPolicy", policy => policy.RequireClaim("Order"))
    .AddPolicy("ShippingPolicy", policy => policy.RequireClaim("Shipping"))
    .AddPolicy("PaymentPolicy", policy => policy.RequireClaim("Payment"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference(o =>
{
    o.WithTheme(ScalarTheme.BluePlanet)
     .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
     .WithPreferredScheme("Bearer");
});


app.UseHttpsRedirection();

app.UseMiddleware<PolicyMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
