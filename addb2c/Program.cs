using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

IdentityModelEventSource.ShowPII = true;
// Add services to the container.
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2C"));

builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy.
    options.FallbackPolicy = options.DefaultPolicy;
});
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("Set-Cookie");
    logging.RequestHeaders.Add("Cookie");
    logging.ResponseHeaders.Add("Set-Cookie");
    logging.ResponseHeaders.Add("Cookie");

    logging.RequestHeaders.Remove("Accept");
    logging.RequestHeaders.Remove("Accept-Encoding");
    logging.RequestHeaders.Remove("Accept-Language");
    logging.RequestHeaders.Remove("sec-ch-ua");
    logging.RequestHeaders.Remove("sec-ch-ua-mobile");
    logging.RequestHeaders.Remove("sec-ch-ua-platform");
    logging.RequestHeaders.Remove("sec-fetch-site");
    logging.RequestHeaders.Remove("sec-fetch-mode");
    logging.RequestHeaders.Remove("sec-fetch-dest");
    logging.RequestHeaders.Remove("Protocol");
    logging.RequestHeaders.Remove("User-Agent");
    logging.RequestHeaders.Remove("Referer");

    logging.ResponseBodyLogLimit = 0;
});

builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

var app = builder.Build();
app.UseHttpLogging();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();