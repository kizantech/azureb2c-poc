using AzureB2C.AuthApi.Interfaces;
using AzureB2C.Data.Context;
using AzureB2C.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace AzureB2C.AuthApi.Services
{
    public class SqlUserInvitationRepository : IInvitationRepository
    {
        private AzureB2cAuthDbContext _dbContext;
        private ILogger<SqlUserInvitationRepository> _logger;

        public SqlUserInvitationRepository(AzureB2cAuthDbContext dbContext, ILogger<SqlUserInvitationRepository> logger)
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
