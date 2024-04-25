namespace AzureB2C.AuthApi.Models;

public class IdentityClaimsPostResponse
{
    public const string version = "1.0.0";
    public int status { get; set; }
    public string userMessage { get; set; }
    public string developerMessage { get; set; }
    public string action { get; set; }
}