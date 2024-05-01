using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AzureB2C.Blazor.Middleware
{
    public static class Interop
    {
        public static ValueTask<object> CreateReport(
            IJSRuntime jsRuntime,
            ElementReference reportContainer,
            string accessToken,
            string embedUrl,
            string embedReportId)
        {
            return jsRuntime.InvokeAsync<object>(
                "ShowMyPowerBI.showReport",
                reportContainer, accessToken, embedUrl,
                embedReportId);
        }
    }
}
