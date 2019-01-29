using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyLi.Services.OAuth.Client
{
   public interface IAuthenticationContext
    {
        #region Properties

        LoginIdentity Identity
        {
            get;
        }

        bool IsAuthenticated
        {
            get;
        }
        #endregion Properties
    }
}
