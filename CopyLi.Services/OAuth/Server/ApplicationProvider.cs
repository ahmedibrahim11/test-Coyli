using CopyLi.Data.Entities.Authentication;
using CopyLi.Utilites.Cryptography;
using Framework.Data.EF;
using Framework.Security.OAuth;
using Framework.Security.OAuth.ApplicationProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyLi.Services.OAuth.Server
{
    public class ApplicationProvider : BaseApplicationProvider
    {
        #region Fields

        private readonly IRepository<Application> _applicationsRepository;
        private readonly IRepository<ApplicationRefreshToken> _applicationTokensRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IRepository<Membership> _membershipRepository;

        #endregion Fields

        #region Constructors

        public ApplicationProvider(
            IEncryptionService encryptionService,
            IRepository<Application> applicationsRepository,
            IRepository<Membership> membershipsRepository,
            IRepository<ApplicationRefreshToken> applicationTokensRepository,
            IUnitOfWorkFactory unitOfWorkFactory)
        {
            _applicationsRepository = applicationsRepository;
            _encryptionService = encryptionService;
            _membershipRepository = membershipsRepository;
            _unitOfWorkFactory = unitOfWorkFactory;
            _applicationTokensRepository = applicationTokensRepository;
        }

        #endregion Constructors

        #region Methods

        public override void DeleteUserToken(IApplication application, string userId, string token)
        {
            using (_unitOfWorkFactory.Create())
            {
                long uid = Convert.ToInt64(userId);
                int aid = Convert.ToInt32(application.Id);

                _applicationTokensRepository.Delete(new Specification<ApplicationRefreshToken>(a => a.ApplicationId == aid && a.MembershipId == uid));
            }
        }

        //public override void SaveUserToken(IApplication application, string userId, string token)
        //{
        //    using (_unitOfWorkFactory.Create())
        //    {
        //        var Id = long.Parse(userId);
        //        var dbUser = _membershipRepository.Single(new Specification<Membership>(m => m.Id == Id));
        //        if (dbUser != null)
        //        {
        //            dbUser.RefreshTokens.Add(new ApplicationRefreshToken()
        //            {
        //                ApplicationId = Convert.ToInt32(application.Id),
        //                RefreshToken = _encryptionService.CreatePasswordHash(token),
        //                IssuedUtc = DateTime.UtcNow,
        //                ExpiresOnUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(application.RefreshTokenTimeout))
        //            });
        //        }
        //    }
        //}

        public override bool ValidateApplication(string id, string secret, out IApplication application)
        {
            application = null;

            var dbApp = _applicationsRepository.Single(new Specification<Application>(a => a.Id.ToString() == id && a.IsActive == true));
            if (dbApp == null)
                return false;
            if (dbApp.Type == ApplicationType.Native)
            {
                var hashedSecret = _encryptionService.CreatePasswordHash(secret, dbApp.SecretSalt);
                if (hashedSecret != dbApp.Secret)
                    return false;
            }
            application = new ApplicationDto()
            {
                AllowedOrigin = dbApp.AllowedOrigin,
                RefreshTokenTimeout = dbApp.TokenLifeTime,
                Id = dbApp.Id.ToString()
            };
            return true;
        }

        public override bool ValidateUserRefreshToken(string token, IApplication application, out IUser user)
        {
            user = null;
            token = _encryptionService.CreatePasswordHash(token);
            var dbMembership = _membershipRepository.Single(new Specification<Membership>(u => u.RefreshTokens.Any((t) => t.RefreshToken == token) && u.IsActive == true));
            using (_unitOfWorkFactory.Create())
            {
                if (dbMembership == null)
                    return false;

                dbMembership.LastLoginDate = DateTime.UtcNow;
            }
            var appid = Convert.ToInt32(application.Id);
            user = new MembershipDto()
            {
                Id = dbMembership.Id.ToString(),
                Username = dbMembership.UserName,
                Roles = new[] { "user" },
                TokenIssueDate = dbMembership.RefreshTokens.First(a => a.ApplicationId == appid).IssuedUtc
            };
            ((MembershipDto)user).SetAddionalResponceParams(dbMembership);
            return true;
        }

        #endregion Methods

    }

}
