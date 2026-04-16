using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using ExploreAzureCloud.Data;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Net.WebRequestMethods;
var builder = WebApplication.CreateBuilder(args);

var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri")!);
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential()
    //,new AzureKeyVaultConfigurationOptions() {
    //ReloadInterval = TimeSpan.FromHours(1) reload SnapShot Every 1 Hour
    //}
    );

// Add services to the container.
builder.Services.AddRazorPages();
//builder.Services.AddApplicationInsightsTelemetry(new Microsoft.ApplicationInsights.AspNetCore.Extensions.ApplicationInsightsServiceOptions
//{
//    ConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]
//});


builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    // get connection string from azure keyvault
    options.UseAzureSql(builder.Configuration.GetConnectionString("AzureSqlConStr"), sqloptions => sqloptions.EnableRetryOnFailure());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

//app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
