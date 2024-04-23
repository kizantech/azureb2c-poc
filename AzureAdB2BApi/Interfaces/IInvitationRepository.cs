﻿using AzureAdB2BApi.Models;

namespace AzureAdB2BApi.Interfaces
{
    public interface IInvitationRepository
    {
        Task CreateUserInvitationAsync(UserInvitation userInvitation);
        Task<UserInvitation> GetPendingUserInvitationAsync(string invitationCode);
        Task DeleteUserInvitationCodeAsync(string invitationCode);
        Task<IList<UserInvitation>> GetPendingUserInvitationsAsync(Guid? customerId = null);
    }
}
