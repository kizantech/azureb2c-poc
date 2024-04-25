using AzureB2C.Data.Model;

namespace AzureB2C.AuthApi.Interfaces
{
    public interface IInvitationRepository
    {
        Task CreateUserInvitationAsync(UserInvitation userInvitation);
        Task<UserInvitation> GetPendingUserInvitationAsync(string invitationCode);
        Task DeleteUserInvitationCodeAsync(string invitationCode);
        Task<IList<UserInvitation>> GetPendingUserInvitationsAsync(Guid? customerId = null);
    }
}
