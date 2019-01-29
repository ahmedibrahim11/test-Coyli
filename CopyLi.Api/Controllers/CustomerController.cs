using CopyLi.Api.Dto;
using CopyLi.Api.Helpers;
using CopyLi.Api.Model.Customer;
using CopyLi.Api.Model.Vendor;
using CopyLi.Data.Entities.Orders;
using CopyLi.Data.Entities.Requests;
using CopyLi.Data.Entities.Users.Customers;
using CopyLi.Data.Entities.Users.Vendors;
using CopyLi.Data.Specifications.RequestVendors;
using Framework.Data.EF;
using Mimo.ControlPanel.Data.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using static CopyLi.Api.Helpers.ExpoPushHelper;

namespace CopyLi.Api.Controllers
{
    [RoutePrefix("customer")]
    public class CustomerController : ApiController
    {
        #region Fields

        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<CustomerLocations> _customerLocationsRepository;
        private readonly IRepository<RequestBid> _requestBidRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Request> _requestRepository;
        private readonly IRepository<RequestVendor> _requestvendorReposirtory;
        private readonly IRepository<Vendor> _vendorRepository;
        private readonly IRepository<RequestHistory> _requestHistoryRepo;
        private readonly IRepository<OrderHistory>_orderHistoryRepo;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        #endregion Fields

        #region Constructors
        public CustomerController(
            IRepository<Customer> customerRepository,
            IRepository<RequestBid> requestBidRepository,
            IRepository<Order> orderRepository,
             IRepository<Request> requestRepository,
            IRepository<RequestVendor> requestvendorReposirtory,
            IRepository<CustomerLocations> customerLocationsRepository,
            IUnitOfWorkFactory unitOfWorkFactory,
            IRepository<Vendor> vendorRepository,
           IRepository<RequestHistory>requestHistoryRepo,
           IRepository<OrderHistory>orderHistoryRepo

)
        {
            _customerRepository = customerRepository;
            _customerLocationsRepository = customerLocationsRepository;
            _requestBidRepository = requestBidRepository;
            _orderRepository = orderRepository;
            _requestvendorReposirtory = requestvendorReposirtory;
            _requestRepository = requestRepository;
            _unitOfWorkFactory = unitOfWorkFactory;
            _vendorRepository = vendorRepository;
            _requestHistoryRepo = requestHistoryRepo;
            _orderHistoryRepo = orderHistoryRepo;
        }

        #endregion Constructors

        #region Testing Push
        [Route("sendPush")]
        [HttpPost]
        public IHttpActionResult SendPush()
        {
            List<NotificationObject> listNotifications = new List<NotificationObject>();
            listNotifications.Add(new NotificationObject
            {
                to = "ExponentPushToken[GFCZKdOZLx9rgXSy2VMGEL]",
                title = "CopyLi",
                body = "Your bid accepted",
                data = new NotificationData
                {
                    title = "CopyLi",
                    body = "Your bid accepted",
                    code = "VEN_BID_ACC",
                    id = "50"
                }
            });
            //tokens.Add("ExponentPushToken[GFCZKdOZLx9rgXSy2VMGEL]");
            //tokens.Add("ExponentPushToken[-EWo24AUzzEGGLNe-rn-b6]");
            var res = ExpoPushHelper.SendPushNotification(listNotifications);
            return Ok(res);
        }
        #endregion

        [Route("{id:long}")]
        public Customer Get(int id)
        {
            var customer = _customerRepository.First(new ById<Customer, long>(id));
            return customer;
        }

        [HttpGet]
        [Route("locations/{customerId:long}")]
        public IHttpActionResult GetLocations(long customerId)
        {
            var spec = new Specification<CustomerLocations>(f => f.CustomerId == customerId && f.Deleted == null);
            var locations = _customerLocationsRepository.Get(spec);

            return Ok(locations);
        }

        [HttpPost]
        [Route("locations")]
        public IHttpActionResult AddLocation(AddEditCustomerLocation addEditCustomerLocation)
        {
            CustomerLocations customerLocation = new CustomerLocations
            {
                CustomerId = addEditCustomerLocation.CustomerId,
                Latitude = addEditCustomerLocation.Latitude,
                Longitude = addEditCustomerLocation.Longitude,
                Title = addEditCustomerLocation.Title
            };
            using (var uow = _unitOfWorkFactory.Create())
            {
                _customerLocationsRepository.Add(customerLocation);
            }
            return Ok();
        }

        [HttpPut]
        [Route("locations")]
        public IHttpActionResult EditLocation(AddEditCustomerLocation addEditCustomerLocation)
        {
            var location = _customerLocationsRepository.First(new ById<CustomerLocations, long>(addEditCustomerLocation.Id));
            if (location != null)
            {
                using (var uow = _unitOfWorkFactory.Create())
                {
                    location.Latitude = addEditCustomerLocation.Latitude;
                    location.Longitude = addEditCustomerLocation.Longitude;
                    location.Title = addEditCustomerLocation.Title;
                }
                return Ok();
            }
            else
                return NotFound();

        }

        [HttpDelete]
        [Route("delete/{locationId:long}")]
        public IHttpActionResult DeleteLocation(long locationId)
        {
            var location = _customerLocationsRepository.First(new ById<CustomerLocations, long>(locationId));
            if (location == null)
                return NotFound();
            else
            {
                using (var uow = _unitOfWorkFactory.Create())
                {
                    location.Deleted = DateTime.UtcNow;
                }
                return Ok();
            }
        }

        [Route("pendingRequests/{customerId:long}")]
        [HttpGet]
        public IHttpActionResult GetCustomerRequests(long customerId)
        {
            var requests = _requestRepository.Get<RequestDto>(new Specification<Request>(s => s.CustomerId == customerId && s.RequestStatus != RequestStatus.order));
            if (requests == null)
                return NotFound();
            return Ok(requests);
        }

        // for specific Request
        [Route("requestBids/{reqId:long}")]
        [HttpGet]
        public IHttpActionResult GetAllBids(long reqId)
        {
            var requestBids = _requestBidRepository.Get<RequestBidDto>(new Specification<RequestBid>(s => s.RequestId == reqId));
            if (requestBids == null)
                return BadRequest();
            return Ok(requestBids);
        }



        [Route("acceptBid/{requestBid:long}")]
        [HttpPost]
        public IHttpActionResult AcceptRequestBid(long requestBid)
        {
            var requestBidObj = _requestBidRepository.First(new ById<RequestBid, long>(requestBid));
            if (requestBidObj == null)
                return BadRequest();

            List<ItemOrder> items = new List<ItemOrder>();
            var myRequest = _requestRepository.First(new ById<Request, long>(requestBidObj.RequestId));

            foreach (var item in myRequest.Items)
            {
                items.Add(new ItemOrder() { Data = item.Data, Name = item.Name, Descrption = item.Descrption, ServiceTypeId = item.ServiceTypeId });
            }
            Order order;
            OrderStatus orderStatus = OrderStatus.Pending;
            if (items.Where(i => i.ServiceTypeId == 7).Any())
            {
                orderStatus = OrderStatus.Closed;
            }
            else
            {
                orderStatus = OrderStatus.Pending;
            }

            using (var uow = _unitOfWorkFactory.Create())
            {
                order = new Order()
                {
                    RequestId = myRequest.Id,
                    CustomerId = myRequest.CustomerId,
                    VendorId = requestBidObj.Request.VendorId,
                    Items = items,
                    CreatedOn = DateTime.Now,
                    CreatedById = myRequest.CustomerId,
                    BidData = requestBidObj.Data,
                    Status = orderStatus,
                    Latitude = myRequest.Latitude,
                    Longitude = myRequest.Longitude
                };
                _orderRepository.Add(order);
                myRequest.RequestStatus = RequestStatus.order;
                RequestHistory requestHistory = new RequestHistory()
                {
                    CreatedOn = DateTime.Now,
                    RequestId = myRequest.Id,
                    CreatedById = myRequest.CustomerId,
                    CustomerId=myRequest.CustomerId,
                    UpdatedById = myRequest.CustomerId,
                    RequestStatus = RequestStatus.order
                };
                _requestHistoryRepo.Add(requestHistory);
            }

            using (var uow=_unitOfWorkFactory.Create())
            {
                OrderHistory orderHistory = new OrderHistory()
                {
                    CreatedById = myRequest.CustomerId,
                    CreatedOn = DateTime.UtcNow,
                    vendorId = requestBidObj.Request.VendorId,
                    OrderStatus = orderStatus,
                    OrderId = order.Id
                };
                _orderHistoryRepo.Add(orderHistory);
            }
             
           
            List<NotificationObject> listNotifications = new List<NotificationObject>();
            if (!String.IsNullOrEmpty(requestBidObj.Request.Vendor.Token))
                listNotifications.Add(new NotificationObject
                {
                    to = requestBidObj.Request.Vendor.Token,
                    title = "CopyLi",
                    body = "Your bid accepted",
                    data = new NotificationData
                    {
                        title = "CopyLi",
                        body = "Your bid accepted",
                        code = "VEN_BID_ACC",
                        id = order.Id.ToString()
                    }
                });
            ExpoPushHelper.SendPushNotification(listNotifications);
            return Ok();
        }

        [Route("allorder/{customerId:long}")]
        [HttpGet]
        public IHttpActionResult GetAllOrder(long customerId)
        {
            var orders = _orderRepository.GetAll<OrderDto>(new Specification<Order>(s => s.CustomerId == customerId));
            return Ok(orders);
        }

        [Route("getCustomerOrders/{customerId:long}/{status:int}")]
        [HttpGet]
        public IHttpActionResult GetCustomerOrders(long customerId, int status)
        {
            var spec = new Specification<Order>(f => f.CustomerId == customerId && f.Status == (OrderStatus)status);
            var order = _orderRepository.Get<OrderDto>(spec).ToList();
            return Ok(order);
        }


        [Route("acceptReview/{orderId:long}")]
        [HttpPost]
        public IHttpActionResult AcceptReview(long orderId)
        {
            var orderObj = _orderRepository.First(new ById<Order, long>(orderId));
            if (orderObj == null)
                return BadRequest();


            using (var uow = _unitOfWorkFactory.Create())
            {
                orderObj.Status = OrderStatus.ReviewAccepted;
            }
            List<NotificationObject> listNotifications = new List<NotificationObject>();
            var vendor = _vendorRepository.First(new ById<Vendor, long>(orderObj.VendorId));
            if (!String.IsNullOrEmpty(vendor.Token))
                listNotifications.Add(new NotificationObject
                {
                    to = vendor.Token,
                    title = "CopyLi",
                    body = "Your sample reviewed by customer",
                    data = new NotificationData
                    {
                        title = "CopyLi",
                        body = "Your sample reviewed by customer",
                        code = "VEN_ORD_REV",
                        id = orderObj.Id.ToString()
                    }
                });
            ExpoPushHelper.SendPushNotification(listNotifications);
            using (var uow=_unitOfWorkFactory.Create())
            {
                OrderHistory orderHistory = new OrderHistory()
                {
                    OrderStatus = OrderStatus.ReviewAccepted,
                    CreatedOn = DateTime.Now,
                    OrderId = orderObj.Id,
                };
                _orderHistoryRepo.Add(orderHistory);
            }
            return Ok();
        }
    }
}