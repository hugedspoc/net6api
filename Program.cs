using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using net60api.Services;

var builder = WebApplication.CreateBuilder(args);

var scopesList = builder.Configuration["SharePoint:Scopes"];

if (string.IsNullOrEmpty(scopesList))
    throw new Exception("The SharePoint Scopes are not specified or configured");

//var initialScopes = builder.Configuration["DownstreamApi:Scopes"]?.Split(' ') ?? builder.Configuration["MicrosoftGraph:Scopes"]?.Split(' ');
var initialScopes = scopesList.Split(' ');

// Add services to the container.
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
                // Used to register ITokenAcquisition for SharePoint
                .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
                //.AddMicrosoftGraph(builder.Configuration.GetSection("MicrosoftGraph"))
                .AddDownstreamWebApi("SharePoint", builder.Configuration.GetSection("SharePoint"))
                .AddInMemoryTokenCaches();
//.AddDistributedTokenCaches();

//builder.Services.AddDistributedMemoryCache();

//builder.Services
//                //.AddSharePointContextFactory();
//                .AddCurrentUserSharePointClientContext();
//                //.AddCurrentUserSharePointClientContextNtlm();

//builder.Services.AddAuthorization(options =>
//{
//    // By default, all incoming requests will be authorized according to the default policy.
//    options.FallbackPolicy = options.DefaultPolicy;
//});

builder.Services
    .AddRazorPages()
    .AddMicrosoftIdentityUI();

var app = builder.Build();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
