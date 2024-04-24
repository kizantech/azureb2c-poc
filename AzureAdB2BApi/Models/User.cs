namespace AzureAdB2BApi.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? InvitationCode { get; set; }
        public Guid CustomerId { get; set; }
        public string DelegatedUserManagementRole { get; set; }
    }
}
