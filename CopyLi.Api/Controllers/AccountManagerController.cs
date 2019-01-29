using CopyLi.Api.Model;
using CopyLi.Data.Entities.Authentication;
using CopyLi.Utilites.Cryptography;
using Framework.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CopyLi.Api.Controllers
{
    [RoutePrefix("account")]

    public class AccountManagerController : ApiController
    {

        private readonly IEncryptionService _encryptionService;

        private readonly IRepository<Membership> _membershipRepo;

        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        public AccountManagerController(IRepository<Membership> membershipRepo,
            IEncryptionService encryptionService,IUnitOfWorkFactory unitOfWorkFactory)
        {
            _membershipRepo = membershipRepo;
            _encryptionService = encryptionService;
            _unitOfWorkFactory = unitOfWorkFactory;
        }


        [HttpPost]
        [Route("changepassword/{Id:long}")]
        public IHttpActionResult ChangePassword(long Id, ChangePasswordModel model)
        {
            var mbership = _membershipRepo.First(new Specification<Membership>(u => u.Id == Id));
            if (mbership == null)
                return NotFound();
            var passwordHash = _encryptionService.CreatePasswordHash(model.OldPassword, mbership.PasswordSalt);
            if (passwordHash != mbership.Password)
                return BadRequest("Old Password is inCorrect");

            using (var uow=_unitOfWorkFactory.Create())
            {
                var enSaltKey = _encryptionService.CreateSaltKey(4);
                var enPassword = _encryptionService.CreatePasswordHash(model.NewPassword, enSaltKey);
                mbership.PasswordSalt = enSaltKey;
                mbership.Password = enPassword;
            }
            return Ok();
        }

    }
}
