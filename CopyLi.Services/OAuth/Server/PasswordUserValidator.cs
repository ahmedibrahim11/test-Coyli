using CopyLi.Data.Entities.Authentication;
using CopyLi.Data.Entities.Users.Admins;
using CopyLi.Data.Entities.Users.Customers;
using CopyLi.Data.Entities.Users.Vendors;
using CopyLi.Data.Specifications.Admins;
using CopyLi.Utilites.Cryptography;
using Framework.Data.EF;
using Framework.Security.OAuth;
using Framework.Security.OAuth.UserValidators;
using Newtonsoft.Json;
using System.Web;

namespace CopyLi.Services.OAuth.Server
{
    public class PasswordUserValidator : BaseUserValidator<CopyLiPasswordGrantType>
    {
        #region Fields
        private readonly IEncryptionService _encryptionService;

        private readonly IRepository<Membership> _membershipsRepository;
        private readonly IRepository<Customer> _customersRepository;
        private readonly IRepository<Vendor> _vendorRepository;
        private readonly IRepository<Admin> _adminRepository;


        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        #endregion Fields

        #region Constructors
        public PasswordUserValidator(
            IRepository<Membership> membershipsRepository,
            IRepository<Customer> customersRepository,
            IRepository<Vendor> vendorRepository,
             IRepository<Admin> adminRepository,
            IUnitOfWorkFactory unitOfWorkFactory,
            IEncryptionService encryptionService)
        {
            _membershipsRepository = membershipsRepository;
            _customersRepository = customersRepository;
            _vendorRepository = vendorRepository;
            _encryptionService = encryptionService;
            _adminRepository = adminRepository;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        #endregion Constructors

        #region Methods

        protected override bool Validate(CopyLiPasswordGrantType param, IApplication application, out IUser member)
        {
            // Check for better solution
            string token = HttpContext.Current.Request.Form["pushNotificationToken"];
            member = null;
            var dbMembership = _membershipsRepository.First(new Specification<Membership>(u => u.UserName.ToLower() == param.Username.ToLower()));
            if (dbMembership == null)
                return false;

            var passwordHash = _encryptionService.CreatePasswordHash(param.Password, dbMembership.PasswordSalt);
            if (passwordHash != dbMembership.Password)
                return false;

            if (dbMembership.IsActive != true)
                return false;
            member = new MembershipDto()
            {
                Id = dbMembership.Id.ToString(),
                Username = dbMembership.UserName,
                //Roles = new[] { dbMembership.Role.Name },
                Roles = new[] { "Admin" },
            };

            ((MembershipDto)member).SetAddionalResponceParams(dbMembership);

            var dbCustomer = _customersRepository.First(new Data.Specifications.Customers.ByMembershipId(dbMembership.Id));
            var dbVendor = _vendorRepository.First(new Data.Specifications.Vendors.ByMembershipId(dbMembership.Id));
            var dbAdmin = _adminRepository.First(new ByMemberShipId(dbMembership.Id));

            // Add Validation for application
            if (dbCustomer == null)
            {
                if (dbVendor == null)
                {
                    if (dbAdmin == null)
                        return false;
                    ((MembershipDto)member).SetAddionalResponceParams(dbAdmin);
                }
                else
                {

                    ((MembershipDto)member).SetAddionalResponceParams(dbVendor);
                    using (var uow = _unitOfWorkFactory.Create())
                    {
                        dbVendor.Token = token;
                    }
                }
            }
            else
            {
                ((MembershipDto)member).SetAddionalResponceParams(dbCustomer);
                using (var uow = _unitOfWorkFactory.Create())
                {
                    dbCustomer.Token = token;
                }
            }
            return true;
        }
        #endregion Methods
    }

    public class CopyLiPasswordGrantType : PasswordGrantType
    {
        public string PushNotificationToken { get; set; }
    }

}
