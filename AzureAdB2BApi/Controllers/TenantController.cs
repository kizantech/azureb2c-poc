using AzureAdB2BApi.Filters;
using AzureAdB2BApi.Interfaces;
using AzureAdB2BApi.Models;
using AzureAdB2BApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureAdB2BApi.Controllers
{
    [BasicAuthenticationFilter]
    [ApiController]
    [Route("[controller]")]
    public class TenantController : ControllerBase
    {
        private readonly ITenantService _tenantService;


        public TenantController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        //[HttpGet]
        //public async Task<Tenant> SetTenant(User user)
        //{
        //    //How do I get user context information from request?
        //    //I get a type conversion error here, need to fix on monday
        //    //return Ok(_tenantService.GetTenant(user));
            
        //}
    }
}
