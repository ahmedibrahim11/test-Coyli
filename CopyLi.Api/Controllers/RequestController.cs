using CopyLi.Api.Helpers;
using CopyLi.Api.Model.Request;
using CopyLi.Data.Entities.Items;
using CopyLi.Data.Entities.Requests;
using CopyLi.Data.Entities.Users.Vendors;
using Framework.Data.EF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Http;
using static CopyLi.Api.Helpers.ExpoPushHelper;

namespace CopyLi.Api.Controllers
{

    [RoutePrefix("request")]
    public class RequestController : ApiController
    {
        #region Fields

        private readonly IRepository<Request> _requestRepository;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IRepository<Vendor> _vendorRepository;
        private readonly IRepository<RequestVendor> _requestVendorRepository;
        private readonly IRepository<RequestHistory> _requestHistoryRepo;

        #endregion Fields

        #region Constructors
        public RequestController(
            IRepository<Request> servicesRepository,
            IUnitOfWorkFactory unitOfWorkFactory,
            IRepository<Vendor> vendorRepository,
            IRepository<RequestVendor> requestVendorRepository,
            IRepository<RequestHistory> requestHistoryRepo)
        {
            _requestRepository = servicesRepository;
            _unitOfWorkFactory = unitOfWorkFactory;
            _vendorRepository = vendorRepository;
            _requestVendorRepository = requestVendorRepository;
            _requestHistoryRepo = requestHistoryRepo;

        }
        #endregion Constructors
        [HttpPost, AllowAnonymous]
        [Route("")]
        public IHttpActionResult AddRequest([FromBody] RequestModel model)
        {
            #region [Vendor]
             
            // check nearest venodr 
            var vendor = _vendorRepository.First(new Specification<Vendor>(s => s.Latitude == model.Latitude && s.Longitude == model.Longitude));
            #endregion
            #region Add Request
            Request request = new Request
            {
                CustomerId = model.CustomerId,
                CreatedOn = DateTime.UtcNow,
                CreatedById = model.CustomerId,
                VendorId = vendor.Id,
                Latitude = model.Latitude,
                Longitude = model.Longitude
            };
            request.Items = new List<Item>();
            foreach (var reqitem in model.Items)
            {
                Item item = new Item
                {
                    Data = reqitem.Data,
                    ServiceTypeId = reqitem.ServiceTypeId
                };
                request.Items.Add(item);
            }
            using (var uow = _unitOfWorkFactory.Create())
            {
                _requestRepository.Add(request);
            }
            using (var uow = _unitOfWorkFactory.Create())
            {
                RequestHistory requestHistory = new RequestHistory()
                {
                    CreatedOn = DateTime.Now,
                    CreatedById = model.CustomerId,
                    RequestId = request.Id,
                    CustomerId = model.CustomerId,
                    RequestStatus = RequestStatus.pending
                };

                _requestHistoryRepo.Add(requestHistory);
            }
            #endregion


            #region [Notifaction]
            List<NotificationObject> listNotifications = new List<NotificationObject>();
            if (!String.IsNullOrEmpty(vendor.Token))
                listNotifications.Add(new NotificationObject
                {
                    to = vendor.Token,
                    title = "CopyLi",
                    body = "New request created",
                    data = new NotificationData
                    {
                        title = "CopyLi",
                        body = "New request created",
                        code = "VEN_NEW_REQ"
                    }
                });
            // ExpoPushHelper.SendPushNotification(listNotifications);
            #endregion


            return Ok();
        }

        [HttpPost]
        [Route("upload/{customerId:long}")]
        public IHttpActionResult Post(long customerId)
        {
            //HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;

            //var membershipId = long.Parse(HttpContext.Current.GetOwinContext().Authentication.User.FindFirst("id").Value);

            if (httpRequest.Files.Count > 0)
            {
                var docfiles = new List<string>();
                var dir = HttpContext.Current.Server.MapPath("~/Uploads");
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    string filename = Path.GetFileNameWithoutExtension(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    filename = filename + DateTime.Now.Ticks.ToString() + extension;
                    string customerFiles = dir + "\\" + customerId;
                    if (!Directory.Exists(customerFiles))
                        Directory.CreateDirectory(customerFiles);
                    postedFile.SaveAs(Path.Combine(customerFiles, filename));
                    string filePath = "/Uploads/1/" + filename;
                    docfiles.Add(filePath);
                }
                return Ok(docfiles);
            }
            else
            {
                return NotFound();
            }


        }

    }
}

