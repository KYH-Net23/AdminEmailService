using Azure.Identity;
using EmailProvider.Data;
using EmailProvider.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
    options.UseMySQL(builder.Configuration["EmailProviderConnectionString"]!)
);

var vaultUrl = new Uri(builder.Configuration["VaultUrl"]!);

builder.Configuration.AddAzureKeyVault(vaultUrl, new DefaultAzureCredential());

builder.Services.AddScoped<EmailService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
