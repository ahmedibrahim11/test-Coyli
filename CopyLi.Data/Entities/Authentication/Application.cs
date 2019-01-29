using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyLi.Data.Entities.Authentication
{
    public class Application : Entity<int>, IAuditable
    {
        public string AllowedOrigin
        {
            set; get;
        }

        public string DisplayName
        {
            set; get;
        }

        public bool IsActive
        {
            set; get;
        }

        public string Secret
        {
            set; get;
        }

        public string SecretSalt
        {
            set; get;
        }

        public int TokenLifeTime
        {
            set; get;
        }

        public long CreatedById
        {
            get;
            set;
        }

        public DateTime CreatedOn
        {
            get;
            set;
        }

        public long? UpdatedById
        {
            get;
            set;
        }

        public DateTime? UpdatedOn
        {
            get;
            set;
        }
        public ApplicationType Type { get; set; }
    }
}
