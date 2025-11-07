using Microsoft.EntityFrameworkCore;
using YourApp.Data;
using YourApp.Services;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

// Configure DbContext with Azure AD authentication
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure();
    });
});

// Add Azure credential for authentication with more explicit options
builder.Services.AddSingleton(new DefaultAzureCredential(new DefaultAzureCredentialOptions
{
    ExcludeEnvironmentCredential = false,
    ExcludeWorkloadIdentityCredential = false,
    ExcludeManagedIdentityCredential = false,
    ExcludeSharedTokenCacheCredential = false,
    ExcludeVisualStudioCredential = false,
    ExcludeVisualStudioCodeCredential = false,
    ExcludeAzureCliCredential = false,
    ExcludeAzurePowerShellCredential = false,
    ExcludeInteractiveBrowserCredential = false
}));

// builder.Services.AddSingleton<EventHubService>();
builder.Services.AddSingleton<SimulatedEventHubService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();

    builder.Services.AddApplicationInsightsTelemetry(options =>
    {
        options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.Run();
