using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using ExploreAzureCloud.Data;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient.AlwaysEncrypted.AzureKeyVaultProvider;
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

// add Always Encryption Keyvault Provider to enc /dec data and access key
// Create the AKV provider using Azure.Identity (recommended)
var azureCredential = new DefaultAzureCredential(); // this line can be reusable here 👆
var akvProvider = new SqlColumnEncryptionAzureKeyVaultProvider(azureCredential);

// Register globally (once per application)
SqlConnection.RegisterColumnEncryptionKeyStoreProviders(
    new Dictionary<string, SqlColumnEncryptionKeyStoreProvider>
    {
        { SqlColumnEncryptionAzureKeyVaultProvider.ProviderName, akvProvider }
    });



// Add services to the container.
builder.Services.AddRazorPages();
//builder.Services.AddApplicationInsightsTelemetry(new Microsoft.ApplicationInsights.AspNetCore.Extensions.ApplicationInsightsServiceOptions
//{
//    ConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]
//});


builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    // get connection string from azure keyvault (this done in runtime, so update db fail Design time) --> sol: DesginTime Factory or Store in Local Secret
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
else
{
    using var scope =  app.Services.CreateScope();
    using var dbcontext=  scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await dbcontext.Database.MigrateAsync();
}
app.UseHttpsRedirection();

app.UseRouting();

//app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
