using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;

namespace Web.Components.Account;

public partial class ForgotPassword
{
    [SupplyParameterFromForm]
    private InputModel Input { get; set; } = new();

    private async Task OnValidSubmitAsync()
    {
        var user = await UserManager.FindByEmailAsync(Input.Email);
        if (user is null || !(await UserManager.IsEmailConfirmedAsync(user)))
        {
            // Don't reveal that the user does not exist or is not confirmed
            RedirectManager.RedirectTo("Account/ForgotPasswordConfirmation");
        }

        // For more information on how to enable account confirmation and password reset please
        // visit https://go.microsoft.com/fwlink/?LinkID=532713
        var code = await UserManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var callbackUrl = NavigationManager.GetUriWithQueryParameters(
            NavigationManager.ToAbsoluteUri("Account/ResetPassword").AbsoluteUri,
            new Dictionary<string, object?> { ["code"] = code });

        await EmailSender.SendPasswordResetLinkAsync(user, Input.Email, HtmlEncoder.Default.Encode(callbackUrl));

        RedirectManager.RedirectTo("Account/ForgotPasswordConfirmation");
    }

    private sealed class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
    }
}
