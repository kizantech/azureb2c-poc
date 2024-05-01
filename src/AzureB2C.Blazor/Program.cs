using AzureB2C.Blazor.Components;
using AzureB2C.Blazor.Models;
using AzureB2C.Blazor.Multitenant;
using AzureB2C.Blazor.Services;
using AzureB2C.Data.Context;
using AzureB2C.Data.Model;
using Blazing.Mvvm;
using Blazing.Mvvm.Infrastructure;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

namespace AzureB2C.Blazor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddMicrosoftIdentityConsentHandler();
            // Loading appsettings.json in C# Model classes
            builder.Services.Configure<AzureAd>(builder.Configuration.GetSection("AzureAd"))
                .Configure<PowerBI>(builder.Configuration.GetSection("PowerBI"));

            builder.Services.AddScoped(typeof(AadService))
                .AddScoped(typeof(PbiEmbedService));
            
            builder.Services.AddDbContext<AzureB2cAuthDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityDatabase"));
            });
            
            builder.Services.AddScoped<UserService>();

            builder.Services.AddMvvmNavigation(options => { options.HostingModel = BlazorHostingModel.Server; });
            
            builder.Services.AddControllersWithViews()
                .AddMicrosoftIdentityUI();
            
            builder.Services.AddCascadingAuthenticationState();
            
            builder.Services.AddMultiTenant<CustomerInfo>()
                .WithClaimStrategy(builder.Configuration["ClaimSettings:TenantIdClaimType"])
                .WithStrategy<BlazorUserStrategy>(ServiceLifetime.Scoped, [])
                .WithEFCoreStore<AzureB2cAuthDbContext, CustomerInfo>();
            
            builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(builder.Configuration, "AzureAdB2C");

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMultiTenant();
            
            app.UseStaticFiles();
            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseAntiforgery();
            app.MapControllers();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();
            app.Run();
        }
    }
}