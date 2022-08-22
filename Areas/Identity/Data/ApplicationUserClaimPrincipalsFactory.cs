using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using TVGuide.Models;

namespace TVGuide.Areas.Identity.Data
{
    public class ApplicationUserClaimPrincipalsFactory : UserClaimsPrincipalFactory<TVGuideUser, IdentityRole>
    {
        public ApplicationUserClaimPrincipalsFactory(UserManager<TVGuideUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
        {

        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(TVGuideUser user)
        {

            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("Keywords", user.Keywords));

            return identity;
        }
    }
}
