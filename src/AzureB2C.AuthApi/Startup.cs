using System.IdentityModel.Tokens.Jwt;
using AzureB2C.AuthApi.Interfaces;
using AzureB2C.AuthApi.Services;
using AzureB2C.AuthApi.Utils;
using AzureB2C.Data.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureADB2C.UI;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Constants = AzureB2C.AuthApi.Utils.Constants;

namespace AzureB2C.AuthApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private const string AzureB2CCorsPolicy = "_azureB2cCorsPolicy";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AzureB2cAuthDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("IdentityDatabase"));
            });
            services.AddScoped<IInvitationRepository, SqlUserInvitationRepository>();
            #pragma warning disable 0618 // AzureADb2CDefaults is obsolete in favor of "Microsoft.Identity.Web"
            var b2cConfigurationSection = Configuration.GetSection("AzureAdB2C");
            var b2cGraphService = new B2cGraphService(
                clientId: b2cConfigurationSection.GetValue<string>(nameof(AzureADB2COptions.ClientId)),
                domain: b2cConfigurationSection.GetValue<string>(nameof(AzureADB2COptions.Domain)),
                clientSecret: b2cConfigurationSection.GetValue<string>(nameof(AzureADB2COptions.ClientSecret)),
                b2CExtensionsAppClientId: b2cConfigurationSection.GetValue<string>("B2cExtensionsAppClientId"));
            services.AddCors(config =>
            {
                config.AddPolicy(name: AzureB2CCorsPolicy,
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .SetPreflightMaxAge(TimeSpan.FromSeconds(200));

                    });
            });
            services.AddTransient<IClaimsTransformation, CustomClaimsTransformation>();
            services.AddSingleton<B2cGraphService>(b2cGraphService);

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.All;
            });

            services.ConfigureSameSiteCookiePolicy();

            // Don't map any standard OpenID Connect claims to Microsoft-specific claims.
            // See https://leastprivilege.com/2017/11/15/missing-claims-in-the-asp-net-core-2-openid-connect-handler/.
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            // Add Azure AD B2C authentication using OpenID Connect.
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = OpenIdConnectDefaults.AuthenticationScheme;

                })
                .AddMicrosoftIdentityWebApp(options =>
                {
                    Configuration.Bind("AzureAdB2C", options);
                    options.TokenValidationParameters.RoleClaimType = b2cGraphService.GetUserAttributeClaimName(Constants.UserAttributes.DelegatedUserManagementRole);
                });

            
            services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                // Don't remove any incoming claims.
                options.ClaimActions.Clear();
                options.NonceCookie.SecurePolicy = CookieSecurePolicy.Always;
                options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
                // Set the "role" claim type to be the "extension_DelegatedUserManagementRole" user attribute.
                options.TokenValidationParameters.RoleClaimType = b2cGraphService.GetUserAttributeClaimName(Constants.UserAttributes.DelegatedUserManagementRole);
            });
            #pragma warning restore 0618


            services.AddRazorPages()
                .AddMicrosoftIdentityUI();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddRouting(options => { options.LowercaseUrls = true; });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseForwardedHeaders();
            //if (env.IsDevelopment())
            //{
                
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}

            if (!env.IsDevelopment())
                app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors(AzureB2CCorsPolicy);
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
