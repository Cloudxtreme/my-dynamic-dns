using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace MyDynamicDns.Filters
{
    public class IdentityBasicAuthenticationAttribute : BasicAuthenticationAttribute
    {
        protected override async Task<IPrincipal> AuthenticateAsync(string userName, string password, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (userName != Settings.UserName || password != Settings.Password)
            {
                // No user with userName/password exists.
                return null;
            }

            // Create a ClaimsIdentity with all the claims for this user.
            var nameClaim = new Claim(ClaimTypes.Name, userName);
            var claims = new List<Claim> { nameClaim };

            // important to set the identity this way, otherwise IsAuthenticated will be false
            // see: http://leastprivilege.com/2012/09/24/claimsidentity-isauthenticated-and-authenticationtype-in-net-4-5/
            var identity = await Task.Run(() => new ClaimsIdentity(claims, AuthenticationTypes.Basic), cancellationToken);

            var principal = new ClaimsPrincipal(identity);
            return principal;
        }
    }
}