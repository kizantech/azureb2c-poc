using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using AzureAdB2BApi.Interfaces;
using AzureAdB2BApi.Services;


namespace AzureAdB2BApi.Filters;

public class BasicAuthenticationFilter : AuthorizationFilterAttribute
{
    private const string Realm = "AzureB2C Demo";
    private readonly ILoginValidation loginValidation;
    public override void OnAuthorization(HttpActionContext actionContext)
    {
        if (actionContext.Request.Headers.Authorization == null)
        {
            actionContext.Response = actionContext.Request
                .CreateResponse(HttpStatusCode.Unauthorized);
            if (actionContext.Response.StatusCode == HttpStatusCode.Unauthorized)
            {
                actionContext.Response.Headers.Add("WWW-Authenticate",
                    string.Format("Basic realm=\"{0}\"", Realm));
            }
        }
        else
        {
            string authenticationToken = actionContext.Request.Headers
                .Authorization.Parameter;
            string decodedAuthenticationToken = Encoding.UTF8.GetString(
                Convert.FromBase64String(authenticationToken));
            string[] usernamePasswordArray = decodedAuthenticationToken.Split(':');
            string username = usernamePasswordArray[0];
            string password = usernamePasswordArray[1];
            if (loginValidation.LoginValidator(username, password))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Accepted);
            }
            else
            {
                actionContext.Response = actionContext.Request
                    .CreateResponse(HttpStatusCode.Unauthorized);
            }
        }
    }
}
