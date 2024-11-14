using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration["EmailProviderConnectionString"]!;

var vaultUrl = new Uri(builder.Configuration["VaultUrl"]!);

builder.Configuration.AddAzureKeyVault(vaultUrl, new DefaultAzureCredential());

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
