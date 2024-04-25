using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AzureB2C.AuthApi.Pages
{
    [Authorize]
    public class IdentityModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
