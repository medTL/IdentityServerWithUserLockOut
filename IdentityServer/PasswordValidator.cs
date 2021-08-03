using System;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityServerAspNetIdentity
{
    public class PasswordValidator: IResourceOwnerPasswordValidator
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public PasswordValidator(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var userName = context.UserName;
            var password = context.Password;

            
            if (await _userManager.FindByNameAsync(userName) is var user && user != null)
            {
               
                var result = await _signInManager.CheckPasswordSignInAsync(user, password, true);
                if ( result.IsLockedOut)
                {
                    context.Result = new GrantValidationResult(
                        TokenRequestErrors.InvalidGrant,
                        "You have been locked from the system");
                }
                if(result.Succeeded)
                {
                    context.Result = new GrantValidationResult(
                        subject: user.Id,
                        authenticationMethod: OidcConstants.AuthenticationMethods.Password);
                }
            }
            
            await Task.CompletedTask;
        }
    }
}