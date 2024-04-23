namespace AzureAdB2BApi.Models
{
    public class UserInvitationRequest
    {
        public Guid CustomerId { get; set; }
        public string DelegatedUserManagementRole { get; set; }
        public int ValidHours { get; set; }
    }
}
