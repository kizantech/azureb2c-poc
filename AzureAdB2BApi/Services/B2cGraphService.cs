using Azure.Identity;
using AzureAdB2BApi.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Graph;

namespace AzureAdB2BApi.Services
{
    public class B2cGraphService
    {
        private readonly GraphServiceClient _graphClient;
        private readonly string _b2cExtensionPrefix;

        public B2cGraphService(string clientId, string domain, string clientSecret, string b2cExtensionsAppClientId)
        {
            // Create the Graph client using an app which refers back to the "regular" Azure AD endpoints
            // of the B2C directory, i.e. not "tenant.b2clogin.com" but "login.microsoftonline.com/tenant".
            // This can then be used to perform Graph API calls using the B2C client application's identity and client credentials.
            var clientSecretCredential = new ClientSecretCredential(domain, clientId, clientSecret);
            _graphClient = new GraphServiceClient(clientSecretCredential);
            _b2cExtensionPrefix = b2cExtensionsAppClientId.Replace("-", "");
        }
        public async Task<IList<User>> GetUsersAsync(string companyId = null)
        {
            // Determine all the user properties to request from the Graph API.
            // Note: there is currently no API to return *all* user properties, only a subset is returned by default
            // and if you need more, you have to explicitly request these as below.
            var companyIdExtensionName = GetUserAttributeExtensionName(Utils.Constants.UserAttributes.CustomerId);
            var delegatedUserManagementRoleExtensionName = GetUserAttributeExtensionName(Utils.Constants.UserAttributes.DelegatedUserManagementRole);
            var invitationCodeExtensionName = GetUserAttributeExtensionName(Utils.Constants.UserAttributes.InviteCode);
            var userPropertiesToRequest = new[] { nameof(Microsoft.Graph.Models.User.Id), nameof(Microsoft.Graph.Models.User.DisplayName), nameof(Microsoft.Graph.Models.User.Identities),
                companyIdExtensionName, delegatedUserManagementRoleExtensionName, invitationCodeExtensionName };

            // Perform the Graph API user request and keep paging through the results until we have them all.
            var users = new List<User>();
            var userRequest = _graphClient.Users.GetAsync(config =>
            {
                config.QueryParameters.Select = userPropertiesToRequest;

                if (!string.IsNullOrWhiteSpace(companyId))
                {
                    // Filter directly in the Graph API call to retrieve only users that are from the specified CompanyId.
                    // Make sure to properly escape single quotes into two consecutive single quotes.
                    config.QueryParameters.Filter = $"{companyIdExtensionName} eq '{companyId.Replace("'", "''")}'";
                }
            });

            while (userRequest != null)
            {
                var usersPage = await userRequest;
                foreach (var user in usersPage.Value)
                {
                    // Check if the user is a "real" B2C user, i.e. one that has signed up through a B2C user flow
                    // and therefore has at least one B2C user attribute in the AdditionalData dictionary.
                    if (user.AdditionalData != null && user.AdditionalData.Any())
                    {
                        users.Add(new User
                        {
                            Id = user.Id,
                            Name = user.DisplayName,
                            InvitationCode = GetUserAttribute<string>(user, invitationCodeExtensionName),
                            CustomerId = GetUserAttribute<Guid>(user, companyIdExtensionName),
                            DelegatedUserManagementRole = GetUserAttribute<string>(user, delegatedUserManagementRoleExtensionName) ?? ""
                        });
                    }
                }

                userRequest = usersPage.OdataNextLink != null ? _graphClient.Users.WithUrl(usersPage.OdataNextLink).GetAsync() : null;
            }
            return users;
        }

        public async Task UpdateUserAsync(User user)
        {
            var userPatch = new Microsoft.Graph.Models.User();
            userPatch.DisplayName = user.Name;
            userPatch.AdditionalData = new Dictionary<string, object>();
            userPatch.AdditionalData[GetUserAttributeExtensionName(Utils.Constants.UserAttributes.CustomerId)] = user.CustomerId;
            userPatch.AdditionalData[GetUserAttributeExtensionName(Utils.Constants.UserAttributes.DelegatedUserManagementRole)] = user.DelegatedUserManagementRole;
            await _graphClient.Users[user.Id].PatchAsync(userPatch);
        }

        public async Task DeleteUserAsync(string userId)
        {
            await _graphClient.Users[userId].DeleteAsync();
        }

        public string GetUserAttributeClaimName(string userAttributeName)
        {
            return $"extension_{userAttributeName}";
        }

        public string GetUserAttributeExtensionName(string userAttributeName)
        {
            return $"extension_{_b2cExtensionPrefix}_{userAttributeName}";
        }

        private T? GetUserAttribute<T>(Microsoft.Graph.Models.User user, string extensionName)
        {
            if (user.AdditionalData == null || !user.AdditionalData.ContainsKey(extensionName))
            {
                return default(T);
            }

            if (user.AdditionalData[extensionName] is T)
            {
                return (T)user.AdditionalData[extensionName];
            }

            throw new InvalidCastException("Type T is invalid for this object.");
        }
    }
}
