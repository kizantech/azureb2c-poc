using System.Text.Json;
using AzureB2C.AuthApi.Interfaces;
using AzureB2C.AuthApi.Services;
using AzureB2C.AuthApi.Utils;
using AzureB2C.Data.Model;
using Microsoft.AspNetCore.Mvc;

namespace AzureB2C.AuthApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvitationController(IInvitationRepository invitationRepository, ILogger<InvitationController> logger, IConfiguration config, B2cGraphService b2cGraphService) : ControllerBase
{
    [HttpPost(nameof(Validate))]
    public async Task<IActionResult> Validate([FromBody] JsonElement body)
    {
        // Azure AD B2C calls into this API when a user is attempting to sign up with an invitation code.
        // We expect a JSON object in the HTTP request which contains the input claims as well as an additional
        // property "ui_locales" containing the locale being used in the user journey (browser flow).
        try
        {
            logger.LogInformation("An invitation code is being validated.");

            // find the invite code on the incoming request.
            var invitationCode = default(string);
            logger.LogInformation("Request Properties:");
            foreach (var element in body.EnumerateObject())
            {
                logger.LogInformation($"- {element.Name}: {element.Value.GetRawText()}");
                // The element name should be the full extension name as seen by the Graph API (e.g. "extension_appid_InvitationCode").
                if (element.Name.Equals("extension_" +
                        Constants.UserAttributes.InviteCode,
                        StringComparison.InvariantCultureIgnoreCase))
                {
                    invitationCode = element.Value.GetString();
                }
            }

            if (string.IsNullOrWhiteSpace(invitationCode) || invitationCode.Length < 10)
            {
                // No invitation code was found in the request or it was too short, return a validation error.
                logger.LogInformation($"The provided invitation code \"{invitationCode}\" is invalid.");
                return GetValidationErrorApiResponse("UserInvitationRedemptionFailed-Invalid", "The invitation code you provided is invalid.");
            }
            else
            {
                // An invitation code was found in the request, look up the user invitation in persistent storage.
                logger.LogInformation($"Looking up user invitation for invitation code \"{invitationCode}\"...");
                var userInvitation = await invitationRepository.GetPendingUserInvitationAsync(invitationCode);
                if (userInvitation == null)
                {
                    // The requested invitation code was not found in persistent storage.
                    logger.LogWarning($"User invitation for invitation code \"{invitationCode}\" was not found.");
                    return GetValidationErrorApiResponse("UserInvitationRedemptionFailed-NotFound", "The invitation code you provided is invalid.");
                }
                else if (userInvitation.ExpiresTime < DateTimeOffset.UtcNow)
                {
                    // The requested invitation code has expired.
                    logger.LogWarning($"User invitation for invitation code \"{invitationCode}\" has expired on {userInvitation.ExpiresTime.ToString("o")}.");
                    return GetValidationErrorApiResponse("UserInvitationRedemptionFailed-Expired", "The invitation code you provided has expired.");
                }
                else
                {
                    // The requested invitation code was found in persistent storage and is valid.
                    logger.LogInformation($"User invitation found for invitation code \"{invitationCode}\".");

                    return GetContinueApiResponse("UserInvitationRedemptionSucceeded", "The invitation code you provided is valid.", userInvitation);
                }
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while processing request body: " + e.ToString());
            return GetBlockPageApiResponse("UserInvitationRedemptionFailed-InternalError", "An error occurred while validating your invitation code, please try again later.");
        }
    }

    private IActionResult GetContinueApiResponse(string code, string userMessage, UserInvitation userInvitation)
    {
        return GetB2cApiConnectorResponse("Continue", code, userMessage, 200, userInvitation);
    }

    private IActionResult GetValidationErrorApiResponse(string code, string userMessage)
    {
        return GetB2cApiConnectorResponse("ValidationError", code, userMessage, 400, null);
    }

    private IActionResult GetBlockPageApiResponse(string code, string userMessage)
    {
        return GetB2cApiConnectorResponse("ShowBlockPage", code, userMessage, 400, null);
    }

    private IActionResult GetB2cApiConnectorResponse(string action, string code, string userMessage, int statusCode, UserInvitation? userInvitation)
    {
        var responseProperties = new Dictionary<string, object>
        {
            { "version", "1.0.0" },
            { "action", action },
            { "userMessage", userMessage },
            { b2cGraphService.GetUserAttributeClaimName(Constants.UserAttributes.CustomerId), userInvitation?.CustomerId ?? Guid.Empty }, // Note: returning just "extension_<AttributeName>" (without the App ID) would work as well!
            { b2cGraphService.GetUserAttributeClaimName(Constants.UserAttributes.DelegatedUserManagementRole), userInvitation?.DelegatedUserManagementRole } // Note: returning just "extension_<AttributeName>" (without the App ID) would work as well!
        };
        if (statusCode != 200)
        {
            // Include the status in the body as well, but only for validation errors.
            responseProperties["status"] = statusCode.ToString();
        }
        return new JsonResult(responseProperties) { StatusCode = statusCode };
    }
}