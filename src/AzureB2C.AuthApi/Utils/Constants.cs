﻿namespace AzureB2C.AuthApi.Utils
{
    public static class Constants
    {
        public static class ClaimTypes
        {
            public const string ObjectId = "http://schemas.microsoft.com/identity/claims/objectidentifier";
        }

        public static class UserAttributes
        {
            public const string DelegatedUserManagementRole = nameof(DelegatedUserManagementRole);
            public const string InviteCode = nameof(InviteCode);
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
