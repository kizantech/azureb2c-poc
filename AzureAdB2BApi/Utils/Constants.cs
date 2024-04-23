namespace AzureAdB2BApi.Utils
{
    public static class Constants
    {
        public static class ClaimTypes
        {
            public const string ObjectId = "oid";
        }

        public static class UserAttributes
        {
            public const string DelegatedUserManagementRole = nameof(DelegatedUserManagementRole);
            public const string InvitationCode = nameof(InvitationCode);
            public const string CustomerId = nameof(CustomerId);
        }

        public static class DelegatedUserManagementRoles
        {
            public const string GlobalAdmin = nameof(GlobalAdmin);
            public const string CompanyAdmin = nameof(CompanyAdmin);
            public const string CompanyUser = nameof(CompanyUser);
        }
    }
}
