using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CopyLi.Services.OAuth.Client
{
   public class AuthenticationContext:IAuthenticationContext
    {
        #region Fields

        private LoginIdentity _identity;

        #endregion Fields

        #region Constructors

        public AuthenticationContext()
        {

        }

        #endregion Constructors


        #region Properties

        public LoginIdentity Identity
        {
            get
            {
                if (_identity == null)
                {
                    _identity = new LoginIdentity();
                    var claims = (ClaimsIdentity)Thread.CurrentPrincipal.Identity;
                    _identity.Id = long.Parse(claims.FindFirst("id").Value);
                    _identity.Name = claims.FindFirst("Name").Value;
                    _identity.Role = claims.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
                }
                return _identity;

            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return Thread.CurrentPrincipal.Identity.IsAuthenticated;
            }
        }

        #endregion Properties
    }
}
