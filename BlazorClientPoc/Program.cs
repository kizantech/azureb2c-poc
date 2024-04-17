using Blazing.Mvvm;
using Blazing.Mvvm.Infrastructure;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using BlazorAppPoc.Components;
using BlazorAppPoc.Contexts;
using BlazorAppPoc.Models.ViewModels;
using BlazorAppPoc.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddMicrosoftIdentityConsentHandler();
builder.Services.AddDbContext<PocDbContext>();
builder.Services.AddScoped<ContextService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<TenantInfo>();
builder.Services.AddTransient<PocViewModel>();

builder.Services.AddMvvmNavigation(options =>
{
    options.HostingModel = BlazorHostingModel.Server;
}); 

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<PocDbContext>();

builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration, "AzureAdB2C");

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();            
builder.Services.AddControllersWithViews()
    .AddMicrosoftIdentityUI();

builder.Services.AddMultiTenant<TenantInfo>()
    .WithClaimStrategy(builder.Configuration["ClaimSettings:TenantIdClaimType"])
    .WithConfigurationStore();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseMultiTenant();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseAntiforgery();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.Run();