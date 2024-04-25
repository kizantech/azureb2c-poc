using AzureAdB2BApi.Contexts;
using Blazing.Mvvm;
using Blazing.Mvvm.Infrastructure;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using BlazorAppPoc.Components;
using BlazorAppPoc.Contexts;
using BlazorAppPoc.Middleware;
using BlazorAppPoc.Multitenant;
using BlazorAppPoc.Services;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TenantInfo = AzureAdB2BApi.Models.TenantInfo;

namespace BlazorAppPoc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddMicrosoftIdentityConsentHandler();
            
            builder.Services.AddDbContext<InvitationsDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityDatabase"));
            });
            
            builder.Services.AddScoped<UserService>();
            
            // builder.Services.TryAddEnumerable(
            //     ServiceDescriptor.Scoped<CircuitHandler, UserCircuitHandler>());

            builder.Services.AddMvvmNavigation(options => { options.HostingModel = BlazorHostingModel.Server; });
            
            builder.Services.AddControllersWithViews()
                .AddMicrosoftIdentityUI();
            
            // builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //     .AddEntityFrameworkStores<PocDbContext>();

            builder.Services.AddCascadingAuthenticationState();
            // builder.Services.AddHttpClient();
            // builder.Services.AddHttpContextAccessor();
            
            builder.Services.AddMultiTenant<TenantInfo>()
                .WithClaimStrategy(builder.Configuration["ClaimSettings:TenantIdClaimType"])
                .WithStrategy<BlazorUserStrategy>(ServiceLifetime.Scoped, [])
                .WithEFCoreStore<InvitationsDbContext, TenantInfo>();
            
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
            //app.UseMiddleware<UserServiceMiddleware>();
            app.MapControllers();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();
            app.Run();
        }
    }
}