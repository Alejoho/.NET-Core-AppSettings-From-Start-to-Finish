using BasicDemo.Components;
using BasicDemo.Options;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;

var builder = WebApplication.CreateBuilder(args);

// Add this to handle missing environment or non-Development cases
if (!builder.Environment.IsDevelopment())
{
    StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);
}

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.Configure<EmailSettingsOptions>(
    builder.Configuration.GetSection("EmailSettings"));

#region Configuration Hierarchy

var memCollection = new Dictionary<string, string>()
{
    { "MainSetting:SubSetting","This is a setting in memory"},
    {"key","value" }
};


builder.Configuration.Sources.Clear();

builder.Configuration.AddJsonFile("appsettings.json",
    optional: false,
    reloadOnChange: true);

builder.Configuration.AddJsonFile(
    $"appsettings.{builder.Environment.EnvironmentName}.json",
    optional: true,
    reloadOnChange: true);

builder.Configuration.AddJsonFile("custom.json", optional: true, reloadOnChange: true);

builder.Configuration.AddXmlFile("custom.xml", optional: true, reloadOnChange: true); ;

builder.Configuration.AddIniFile("custom.ini", optional: true, reloadOnChange: true);

builder.Configuration.AddInMemoryCollection(memCollection);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>(true, true);
}

builder.Configuration.AddEnvironmentVariables();

builder.Configuration.AddCommandLine(args);



#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseStaticFiles();
}


app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();