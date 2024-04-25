namespace AzureB2C.AuthApi.Models
{
    public class UserInvitationRequest
    {
        public Guid CustomerId { get; set; }
        public string DelegatedUserManagementRole { get; set; }
        public int ValidHours { get; set; }
    }
}
