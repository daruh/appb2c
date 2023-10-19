using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.IdentityModel.Logging;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2C"));

IdentityModelEventSource.ShowPII = true;

builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("Set-Cookie");
    logging.RequestHeaders.Add("Cookie");
    logging.ResponseHeaders.Add("Set-Cookie");
    logging.ResponseHeaders.Add("Cookie");
    logging.ResponseBodyLogLimit = 0;

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

});

builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

var app = builder.Build();
app.UseHttpLogging();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
