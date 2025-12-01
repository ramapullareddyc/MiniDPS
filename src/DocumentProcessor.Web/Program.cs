using DocumentProcessor.Web.Components;
using DocumentProcessor.Web.Data;
using DocumentProcessor.Web.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

// Configure database connection
string connectionString = string.Empty;
try
{
    var secretsService = new SecretsService();
    string secretJson;

    // First: Try to get secret with "target" in name (Postgres)
    try
    {
        secretJson = await secretsService.GetSecretAsync("atx-db-modernization-atx-db-modernization-1-target");
        if (!string.IsNullOrWhiteSpace(secretJson))
        {
            var username = secretsService.GetFieldFromSecret(secretJson, "username");
            var password = secretsService.GetFieldFromSecret(secretJson, "password");
            var host = secretsService.GetFieldFromSecret(secretJson, "host");
            var port = secretsService.GetFieldFromSecret(secretJson, "port");
            var dbname = "postgres";
            connectionString = $"Host={host};Port={port};Database={dbname};Username={username};Password={password};";
        }
        else throw new Exception("Secret was empty");
    }
    catch
    {
        // Second: Try to get secret by description (SQL Server)
        secretJson = await secretsService.GetSecretByDescriptionPrefixAsync("Password for RDS MSSQL used for MAM319.");
        if (!string.IsNullOrWhiteSpace(secretJson))
        {
            var username = secretsService.GetFieldFromSecret(secretJson, "username");
            var password = secretsService.GetFieldFromSecret(secretJson, "password");
            var host = secretsService.GetFieldFromSecret(secretJson, "host");
            var port = secretsService.GetFieldFromSecret(secretJson, "port");
            var dbname = secretsService.GetFieldFromSecret(secretJson, "dbname");
            connectionString = $"Host={host};Port={port};Database={dbname};Username={username};Password={password};";
        }
        else throw new Exception("Failed to retrieve database credentials from Secrets Manager");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Warning: Could not load connection string from AWS Secrets Manager: {ex.Message}");
    Console.WriteLine("Falling back to appsettings.json connection string");
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Host=localhost;Database=postgres;Username=postgres;Password=postgres;";
}

builder.Services.AddDbContext<AppDbContext>(o => o.UseNpgsql(connectionString));
builder.Services.AddScoped<FileStorageService>();
builder.Services.AddScoped<AIService>();
builder.Services.AddScoped<DocumentProcessingService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.EnsureCreatedAsync();
}

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseExceptionHandler("/Error");

app.UseHttpsRedirection();
app.UseAntiforgery();
app.UseStaticFiles();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
