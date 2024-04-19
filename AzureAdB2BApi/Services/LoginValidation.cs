using AzureAdB2BApi.Interfaces;
using AzureAdB2BApi.Models;

namespace AzureAdB2BApi.Services;

public class LoginValidation : ILoginValidation
{
    public bool LoginValidator(string username, string password)
    {
        if (username == "BasicAuthDemo" && password == "Api123")
        {
            return true;
        }
        return false;
    }

   
}