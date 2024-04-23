using System.ComponentModel.DataAnnotations;

namespace AzureAdB2BApi.Models
{
    public class UserInvitation
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string InvitationCode { get; set; }
        public Guid CustomerId { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset ExpiresTime { get; set; }
        public string DelegatedUserManagementRole { get; set; }
    }
}
