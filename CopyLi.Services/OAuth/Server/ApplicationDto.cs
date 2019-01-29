using Framework.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyLi.Services.OAuth.Server
{
    public class ApplicationDto : IApplication
    {
        #region Properties

        public Dictionary<string, string> AdditionalResponseParameters
        {
            get;
            set;
        }

        public Dictionary<string, string> AdditionalTokenParameters
        {
            get;
            set;
        }

        public string AllowedOrigin
        {
            get; set;
        }

        public string Id
        {
            set; get;
        }

        public string Name
        {
            get;
            set;
        }

        public int RefreshTokenTimeout
        {
            get; set;
        }

        #endregion Properties
    }
}
