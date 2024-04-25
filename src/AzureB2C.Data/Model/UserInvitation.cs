using System.ComponentModel.DataAnnotations;

namespace AzureB2C.Data.Model
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
