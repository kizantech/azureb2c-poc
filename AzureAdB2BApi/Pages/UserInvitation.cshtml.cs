using AzureAdB2BApi.Interfaces;
using AzureAdB2BApi.Models;
using AzureAdB2BApi.Services;
using AzureAdB2BApi.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AzureAdB2BApi.Pages
{
    public class UserInvitationModel : PageModel
    {
        private readonly ILogger<UserInvitationModel> logger;
        private readonly IInvitationRepository userInvitationRepository;
        private readonly B2cGraphService b2cGraphService;
        public bool ShowGlobalAdminUserInvitation { get; set; }
        public bool CanManageUserInvitations { get; set; }
        public bool CanSelectGlobalAdmins { get; set; }
        public bool CanSelectCompany { get; set; }
        public string GlobalAdminInvitationCode { get; set; } = InviteCodeGenerator.GenerateInviteCode();
        public IList<UserInvitation> PendingUserInvitations { get; set; }

        public UserInvitationModel(ILogger<UserInvitationModel> logger, IInvitationRepository userInvitationRepository, B2cGraphService b2cGraphService)
        {
            this.logger = logger;
            this.userInvitationRepository = userInvitationRepository;
            this.b2cGraphService = b2cGraphService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var allUsers = await this.b2cGraphService.GetUsersAsync();
            if (!allUsers.Any())
            {
                var adminInvite = await userInvitationRepository.GetPendingUserInvitationsAsync(Guid.Empty);
                if (adminInvite.Any())
                {
                    GlobalAdminInvitationCode = adminInvite.First().InvitationCode;
                    this.ShowGlobalAdminUserInvitation = true;
                    this.CanManageUserInvitations = false;
                }
                else
                {
                    // If there aren't any users yet, allow anonymous access to bootstrap the initial global admin.
                    var globalAdminUserInvitation = new UserInvitation
                    {
                        InvitationCode = GlobalAdminInvitationCode,
                        CustomerId = Guid.Empty,
                        DelegatedUserManagementRole = Constants.DelegatedUserManagementRoles.GlobalAdmin,
                        CreatedTime = DateTimeOffset.UtcNow,
                        ExpiresTime = DateTimeOffset.UtcNow.AddYears(1),
                        CreatedBy = "System",
                        Id = Guid.NewGuid()
                    };
                    await this.userInvitationRepository.CreateUserInvitationAsync(globalAdminUserInvitation);
                    this.ShowGlobalAdminUserInvitation = true;
                    this.CanManageUserInvitations = false;
                }
            }
            else
            {
                if (!this.User.Identity.IsAuthenticated)
                {
                    // Force the user to sign in if they're not authenticated at this point.
                    return this.Challenge();
                }
                this.ShowGlobalAdminUserInvitation = false;

                if (this.User.IsInRole(Constants.DelegatedUserManagementRoles.GlobalAdmin))
                {
                    this.CanManageUserInvitations = true;
                    this.CanSelectGlobalAdmins = true;
                    this.CanSelectCompany = true;
                    this.PendingUserInvitations = await this.userInvitationRepository.GetPendingUserInvitationsAsync(); ;
                }
                else if (this.User.IsInRole(Constants.DelegatedUserManagementRoles.CompanyAdmin))
                {
                    this.CanManageUserInvitations = true;
                    this.CanSelectGlobalAdmins = false;
                    this.CanSelectCompany = false;
                    var userCompanyId = this.User.FindFirst(this.b2cGraphService.GetUserAttributeClaimName(Constants.UserAttributes.CustomerId))?.Value;
                    this.PendingUserInvitations = await this.userInvitationRepository.GetPendingUserInvitationsAsync(Guid.Parse(userCompanyId)); ;
                }
                else
                {
                    this.CanManageUserInvitations = false;
                }

                this.PendingUserInvitations = this.PendingUserInvitations?.OrderBy(u => u.CustomerId).ThenBy(u => u.DelegatedUserManagementRole).ToArray();
            }
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync(UserInvitationRequest userInvitationRequest)
        {
            // Check that the current user has permissions to create the invitation.
            if (!this.User.IsInRole(Constants.DelegatedUserManagementRoles.GlobalAdmin) && !this.User.IsInRole(Constants.DelegatedUserManagementRoles.CompanyAdmin))
            {
                return this.Unauthorized();
            }

            var userInvitation = new UserInvitation
            {
                InvitationCode = InviteCodeGenerator.GenerateInviteCode(),
                CustomerId = userInvitationRequest.CustomerId,
                DelegatedUserManagementRole = userInvitationRequest.DelegatedUserManagementRole,
                CreatedTime = DateTimeOffset.UtcNow,
                ExpiresTime = DateTimeOffset.UtcNow.AddHours(userInvitationRequest.ValidHours),
                CreatedBy = this.User.FindFirst(Constants.ClaimTypes.ObjectId)?.Value
            };

            if (this.User.IsInRole(Constants.DelegatedUserManagementRoles.CompanyAdmin))
            {
                // For company admins, ensure to set the newly invited user's company to the inviting user's company.
                var userCompanyId = this.User.FindFirst(this.b2cGraphService.GetUserAttributeClaimName(Constants.UserAttributes.CustomerId))?.Value;
                userInvitation.CustomerId = Guid.Parse(userCompanyId);

                // Also ensure the invited user isn't elevated to a global admin.
                if (string.Equals(userInvitation.DelegatedUserManagementRole, Constants.DelegatedUserManagementRoles.GlobalAdmin, StringComparison.InvariantCultureIgnoreCase))
                {
                    userInvitation.DelegatedUserManagementRole = Constants.DelegatedUserManagementRoles.CompanyAdmin;
                }
            }
            await this.userInvitationRepository.CreateUserInvitationAsync(userInvitation);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteUserInvitationAsync(string invitationCode)
        {
            // Check that the current user has permissions to delete the invitation.
            if (!this.User.IsInRole(Constants.DelegatedUserManagementRoles.GlobalAdmin) && !this.User.IsInRole(Constants.DelegatedUserManagementRoles.CompanyAdmin))
            {
                return this.Unauthorized();
            }

            // In a real production scenario, additional validation would be needed here especially for Company Admins:
            // - Ensure that the user invitation being deleted is of the same company as the current user.
            // - ...

            await this.userInvitationRepository.DeleteUserInvitationCodeAsync(invitationCode);
            return RedirectToPage();
        }
    }
}
