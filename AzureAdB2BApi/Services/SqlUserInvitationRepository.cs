using AzureAdB2BApi.Contexts;
using AzureAdB2BApi.Interfaces;
using AzureAdB2BApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AzureAdB2BApi.Services
{
    public class SqlUserInvitationRepository : IInvitationRepository
    {
        private InvitationsDbContext _dbContext;
        private ILogger<SqlUserInvitationRepository> _logger;

        public SqlUserInvitationRepository(InvitationsDbContext dbContext, ILogger<SqlUserInvitationRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task CreateUserInvitationAsync(UserInvitation userInvitation)
        {
            await _dbContext.UserInvitations.AddAsync(userInvitation);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<UserInvitation> GetPendingUserInvitationAsync(string invitationCode)
        {
            var invitation = await _dbContext.UserInvitations.SingleOrDefaultAsync(i => i.InvitationCode == invitationCode);
            return invitation ?? throw new KeyNotFoundException("Invalid Invite Code.");
        }

        public async Task DeleteUserInvitationCodeAsync(string invitationCode)
        {
            var invite =  await _dbContext.UserInvitations.Where(i => i.InvitationCode == invitationCode).FirstOrDefaultAsync();
            if (invite != null)
                _ = _dbContext.Remove(invite);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IList<UserInvitation>> GetPendingUserInvitationsAsync(Guid? customerId = null)
        {
            if (customerId == null)
            {
                return await _dbContext.UserInvitations.ToListAsync();
            }

            return await _dbContext.UserInvitations.Where(c => c.CustomerId == customerId).ToListAsync();
        }
    }
}
