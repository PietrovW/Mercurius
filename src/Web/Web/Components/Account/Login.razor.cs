using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Web.Components.Account;

public partial class Login
{
    private string? errorMessage;

    [CascadingParameter]
    private HttpContext HttpContext { get; set; } = default!;

    [SupplyParameterFromForm]
    private InputModel Input { get; set; } = new();

    [SupplyParameterFromQuery]
    private string? ReturnUrl { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (HttpMethods.IsGet(HttpContext.Request.Method))
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        }
    }

    public async Task LoginUser()
    {
        // This doesn't count login failures towards account lockout
        // To enable password failures to trigger account lockout, set lockoutOnFailure: true
        var result = await SignInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            Logger.LogInformation("User logged in.");
            RedirectManager.RedirectTo(ReturnUrl);
        }
        else if (result.RequiresTwoFactor)
        {
            RedirectManager.RedirectTo(
                "Account/LoginWith2fa",
                new() { ["returnUrl"] = ReturnUrl, ["rememberMe"] = Input.RememberMe });
        }
        else if (result.IsLockedOut)
        {
            Logger.LogWarning("User account locked out.");
            RedirectManager.RedirectTo("Account/Lockout");
        }
        else
        {
            errorMessage = "Error: Invalid login attempt.";
        }
    }

    private sealed class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
