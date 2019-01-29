using CopyLi.Data.Entities.Authentication;
using CopyLi.Data.Entities.Users.Customers;
using CopyLi.Data.Entities.Users.Vendors;
using CopyLi.Services.Models;
using CopyLi.Utilites.Cryptography;
using Framework.Data.EF;
using Framework.Security.OAuth;
using Framework.Security.OAuth.UserValidators;
using System;
using System.Text.RegularExpressions;

namespace CopyLi.Services.OAuth.Server
{
    public class RegisterUserValidator : BaseUserValidator<RegisterUserModel>
    {
        private readonly IRepository<Membership> _membershipRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IEncryptionService _encryptionService;

        public RegisterUserValidator(
           IUnitOfWorkFactory unitOfWorkFactory,
           IRepository<Membership> membershipRepository,
           IRepository<Customer> customerRepository,
           IRepository<Role> roleRepository,
           IEncryptionService encryptionService
           )
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _membershipRepository = membershipRepository;
            _customerRepository = customerRepository;
            _encryptionService = encryptionService;
            _roleRepository = roleRepository;

        }
        protected override bool Validate(RegisterUserModel param, IApplication application, out IUser user)
        {
            user = null;
            Customer customer;

            if (param.FullName == null || param.FullName == string.Empty)
                return false;

            if (param.Password == null || param.Password == string.Empty)
                return false;

            if (param.Email == null || param.Email == string.Empty)
                return false;

            if (param.Mobile == null || param.Mobile == string.Empty)
                return false;

            var dbMembership = _membershipRepository.First(new Specification<Membership>(u => u.UserName.ToLower() == param.Email.ToLower()));
            if (dbMembership != null)
                return false;

            using (var uow = _unitOfWorkFactory.Create())
            {
                var enSaltKey = _encryptionService.CreateSaltKey(8);
                var enPassword = _encryptionService.CreatePasswordHash(param.Password, enSaltKey);

                dbMembership = new Membership()
                {
                    PasswordSalt = enSaltKey,
                    Password = enPassword,
                    CreatedOn = DateTime.UtcNow,
                    IsActive = true,
                    UserName = param.Email,
                    Role = _roleRepository.First(new Specification<Role>(r => r.Name == "User"))
                };

                customer = new Customer()
                {
                    Membership = dbMembership,
                    Name = param.FullName,
                    Mobile = param.Mobile,
                    Email = param.Email
                };
                _customerRepository.Add(customer);
            }
            user = new MembershipDto()
            {
                Id = dbMembership.Id.ToString(),
                Username = dbMembership.UserName,
                Roles = new[] { dbMembership.Role.Name }
            };
            ((MembershipDto)user).SetAddionalResponceParams(dbMembership);
            return true;
        }
    }
}
