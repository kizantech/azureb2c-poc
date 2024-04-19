using AzureAdB2BApi.Filters;
using AzureAdB2BApi.Interfaces;
using AzureAdB2BApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace AzureAdB2BApi.Controllers
{
    [BasicAuthenticationFilter]
    [ApiController]
    [Route("[controller]")]
    public class TenantController : ControllerBase
    {
        private readonly ILoginValidation _userValidation;

        public TenantController(ILoginValidation userValidation)
        {
            _userValidation = userValidation;
        }

        [HttpGet]
        public async Task<Tenant> GetTenant(string userName, string password)
        {
            return Ok(_userValidation.LoginValidator(userName, password));
        }
    }
}
