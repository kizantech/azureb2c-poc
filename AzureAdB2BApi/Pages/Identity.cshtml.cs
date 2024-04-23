using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AzureAdB2BApi.Pages
{
    [Authorize]
    public class IdentityModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
