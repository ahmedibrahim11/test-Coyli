using CopyLi.Api.Dto;
using CopyLi.Api.Helpers;
using CopyLi.Api.Model.Vendor;
using CopyLi.Data.Entities.Authentication;
using CopyLi.Data.Entities.Orders;
using CopyLi.Data.Entities.Requests;
using CopyLi.Data.Entities.Users.Customers;
using CopyLi.Data.Entities.Users.Vendors;
using CopyLi.Data.Specifications.RequestVendors;
using CopyLi.Utilites.Cryptography;
using Framework.Data.EF;
using Mimo.ControlPanel.Data.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using static CopyLi.Api.Helpers.ExpoPushHelper;

namespace CopyLi.Api.Controllers
{
    [RoutePrefix("vendor")]
    public class VendorController : ApiController
    {
        #region Fields
        private readonly IEncryptionService _encryptionService;
        private readonly IRepository<Vendor> _vendorRepository;
        private readonly IRepository<Request> _requestRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<RequestVendor> _requestVendorRepository;
        private readonly IRepository<RequestBid> _requestBidRepository;

        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Membership> _membershipRepository;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<OrderHistory> _orderHistoryRepo;

        #endregion Fields

        #region Constructors
        public VendorController(
            IRepository<Vendor> vendorRepository,
            IRepository<Request> requestRepository,
            IRepository<RequestVendor> requestVendorRepository,
            IRepository<RequestBid> requestBidRepository,
            IRepository<Membership> membershipRepository,
            IRepository<Role> roleRepository,
            IEncryptionService encryptionService,
            IUnitOfWorkFactory unitOfWorkFactory,
            IRepository<Order> orderRepository,
            IRepository<Customer> customerRepository,
            IRepository<OrderHistory> orderHistoryRepo)
        {
            _vendorRepository = vendorRepository;
            _requestRepository = requestRepository;
            _requestVendorRepository = requestVendorRepository;
            _roleRepository = roleRepository;
            _requestBidRepository = requestBidRepository;
            _membershipRepository = membershipRepository;
            _unitOfWorkFactory = unitOfWorkFactory;
            _encryptionService = encryptionService;
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _orderHistoryRepo = orderHistoryRepo;
        }

        #endregion Constructors
        [Route("{id:long}")]
        public Vendor Get(int id)
        {
            var vendor = _vendorRepository.First(new ById<Vendor, long>(id));
            return vendor;
        }
        [HttpGet]
        [Route("vendors")]
        [ResponseType(typeof(VendorDto[]))]

        public IEnumerable<VendorDto> GetAllVendors()
        {
            var vendor = _vendorRepository.GetAll<VendorDto>();
            return vendor;
        }
        [Route("create")]
        [HttpPost]
        public IHttpActionResult CreateVendor(CreateVendorModel model)
        {
            var enSaltKey = _encryptionService.CreateSaltKey(4);
            var enPassword = _encryptionService.CreatePasswordHash(model.Password, enSaltKey);
            if (model == null)
                return NotFound();
            Vendor vendor = new Vendor()
            {
                Email = model.Email,
                Mobile = model.Mobile,
                Name = model.Name,
                Membership = new Membership()
                {
                    PasswordSalt = enSaltKey,
                    Password = enPassword,
                    UserName = model.Email,
                    CreatedOn = DateTime.Now,
                    Role = _roleRepository.First(new Specification<Role>(r => r.Name == "Vendor"))
                }
            };
            using (var uow = _unitOfWorkFactory.Create())
            {
                _vendorRepository.Add(vendor);
            }
            return Ok(vendor);

        }

        [Route("edit/{vendorId:long}")]
        [HttpPut]
        public IHttpActionResult EditVendor(long vendorId, EditVendorModel model)
        {
            if (model == null)
                return NotFound();
            var vendor = _vendorRepository.First(new ById<Vendor, long>(vendorId));
            if (vendor == null)
                return NotFound();

            using (var uow = _unitOfWorkFactory.Create())
            {
                vendor.Email = model.Email;
                vendor.Mobile = model.Mobile;
                vendor.Name = model.Name;
            };
            return Ok();
        }

        [Route("location")]
        [HttpPut]
        public IHttpActionResult Location(AddEditVendorLocationModel addEditVendorLocationModel)
        {
            var vendor = _vendorRepository.First(new ById<Vendor, long>(addEditVendorLocationModel.VendorId));
            if (vendor != null)
            {
                using (var uow = _unitOfWorkFactory.Create())
                {
                    vendor.Latitude = addEditVendorLocationModel.Latitude;
                    vendor.Longitude = addEditVendorLocationModel.Longitude;
                    vendor.Location = addEditVendorLocationModel.Location;
                }
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [Route("delete/{Id:long}")]
        [HttpDelete]
        public IHttpActionResult DeleteVendor(long Id)
        {
            var vendor = _vendorRepository.First(new ById<Vendor, long>(Id));
            if (vendor == null)
                return NotFound();
            using (var uow = _unitOfWorkFactory.Create())
            {
                _vendorRepository.Delete(vendor);
            }
            return Ok();
        }

        [Route("activate/{Id:long}")]
        [HttpPut]
        public IHttpActionResult ActivateVendor(long Id)
        {
            var vendor = _vendorRepository.First(new ById<Vendor, long>(Id));
            if (vendor == null)
                return NotFound();
            var membership = _membershipRepository.First(new ById<Membership, long>(vendor.MembershipId));
            if (membership == null)
                return NotFound();
            using (var uow = _unitOfWorkFactory.Create())
            {
                membership.IsActive = true;
            }
            return Ok();
        }

        [Route("request/{id:long}")]
        public IHttpActionResult GetPendingRequests(long id)
        {
            List<RequestDto> PendingNotBidRequests = new List<RequestDto>();
            var currentvendor = _vendorRepository.First<VendorDto>(new ById<Vendor, long>(id));
            if (currentvendor == null)
                return NotFound();
            var spec = _requestRepository.Get<RequestDto>(new Specification<Request>(s=>s.RequestStatus==RequestStatus.pending && s.VendorId==currentvendor.Id));

            foreach (var item in spec)
            {
                var requestBids = _requestBidRepository.GetAll().ToList();
                bool checkRequest = requestBids.Any(b => b.RequestId == item.Id);
                if (!checkRequest)
                {
                    PendingNotBidRequests.Add(item);
                }
            }
            return Ok(PendingNotBidRequests);
        }

        [Route("requestById/{id:long}")]
        [HttpGet]
        public IHttpActionResult GetRequestById(long id)
        {
            var request = _requestRepository.First<RequestDto>(new ById<Request, long>(id));
            return Ok(request);
        }

        [Route("bidRequest/{vendorId:long}")]
        [HttpPost]
        public IHttpActionResult AddBidRequest(long vendorId, BidModel bidModel)
        {
            var request = _requestRepository.First<RequestDto>(new ById<Request, long>(bidModel.RequestId));
            if (request == null)
                return BadRequest();
            List<NotificationObject> listNotifications = new List<NotificationObject>();
            using (var uow = _unitOfWorkFactory.Create())
            {
                RequestBid requestBid = new RequestBid()
                {
                    Data = bidModel.BidData,
                    RequestId = request.Id,
                    Price = 0.0m,
                };
                _requestBidRepository.Add(requestBid);
            }
          
            if (!String.IsNullOrEmpty(request.CustomerToken))
                listNotifications.Add(new NotificationObject
                {
                    to = request.CustomerToken,
                    title = "CopyLi",
                    body = "Vendor add new bid",
                    data = new NotificationData
                    {
                        title = "CopyLi",
                        body = "Your bid accepted",
                        code = "CUS_NEW_BID",
                        id = request.Id.ToString()
                    }
                });
            ExpoPushHelper.SendPushNotification(listNotifications);
            return Ok();
        }

        [Route("getVendorOrders/{vendorId:long}/{status:int}")]
        [HttpGet]
        public IHttpActionResult GetVendorOrders(long vendorId, int status)
        {
            var spec = new Specification<Order>(f => f.VendorId == vendorId && f.Status == (OrderStatus)status);
            var pendingOrders = _orderRepository.Get<OrderDto>(spec).ToList();
            return Ok(pendingOrders);
        }

        [Route("submitforreview/{vendorId:long}")]
        [HttpPost]
        public IHttpActionResult SubmitForReview(long vendorId, ReviewModel reviewModel)
        {
            var spec = new Specification<Order>(f => f.VendorId == vendorId && f.Id == reviewModel.OrderId);

            var order = _orderRepository.First(spec);
            if (order != null)
            {
                using (var uow = _unitOfWorkFactory.Create())
                {
                    order.ReviewData = reviewModel.ReviewData;
                    order.Status = OrderStatus.WaitForReview;
                }
                var customer = _customerRepository.First(new ById<Customer, long>(order.CustomerId));

                List<NotificationObject> listNotifications = new List<NotificationObject>();
                if (!String.IsNullOrEmpty(customer.Token))
                    listNotifications.Add(new NotificationObject
                    {
                        to = customer.Token,
                        title = "CopyLi",
                        body = "vendor submit your order",
                        data = new NotificationData
                        {
                            title = "CopyLi",
                            body = "vendor submit your order",
                            code = "CUS_WAIT_REVIEW",
                            id = order.Id.ToString()
                        }
                    });
                ExpoPushHelper.SendPushNotification(listNotifications);
                using (var uow=_unitOfWorkFactory.Create())
                {
                    OrderHistory orderHistory = new OrderHistory()
                    {
                        OrderStatus = OrderStatus.WaitForReview,
                        CreatedOn = DateTime.Now,
                        OrderId = order.Id,
                    };
                    _orderHistoryRepo.Add(orderHistory);
                }
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [Route("submitOrder/{orderId:long}")]
        [HttpPost]
        public IHttpActionResult SubmitOrder(long orderId)
        {
            var spec = new Specification<Order>(f => f.Id == orderId);

            var order = _orderRepository.First(spec);
            if (order != null)
            {
                if (order.Items.Where(item => item.ServiceTypeId == 7).Any())
                {
                    using (var uow = _unitOfWorkFactory.Create())
                    {
                        order.Status = OrderStatus.Closed;
                    }
                    return Ok();
                }
                else
                {
                    if (order.Status == OrderStatus.Pending)
                    {
                        using (var uow = _unitOfWorkFactory.Create())
                        {
                            order.Status = OrderStatus.OnTheWay;
                        }
                    }
                    if (order.Status == OrderStatus.ReviewAccepted)
                    {
                        using (var uow = _unitOfWorkFactory.Create())
                        {
                            order.Status = OrderStatus.OnTheWay;
                        }
                    }
                    else if (order.Status == OrderStatus.OnTheWay)
                    {
                        using (var uow = _unitOfWorkFactory.Create())
                        {
                            order.Status = OrderStatus.Closed;
                        }
                    }
                    using (var uow = _unitOfWorkFactory.Create())
                    {
                        OrderHistory orderHistory = new OrderHistory()
                        {
                            OrderStatus =order.Status,
                            CreatedOn = DateTime.Now,
                            OrderId = order.Id,
                        };
                        _orderHistoryRepo.Add(orderHistory);
                    }
                    return Ok();
                }

            }
            else
            {
                return NotFound();
            }
        }

    }
}