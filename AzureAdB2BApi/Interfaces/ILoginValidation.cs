namespace AzureAdB2BApi.Interfaces;

public interface ILoginValidation
{
    public bool LoginValidator(string username, string password);
}