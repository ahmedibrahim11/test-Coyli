using CopyLi.Api.Dto;
using CopyLi.Data.Entities.Requests;
using Framework.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CopyLi.Api.Controllers
{
    [RoutePrefix("superAdmin")]
    public class SuperAdminController : ApiController
    {
        private readonly IRepository<RequestHistory> _requestHistoryRepo;
        public SuperAdminController(IRepository<RequestHistory> requestHistoryRepo)
        {
            _requestHistoryRepo = requestHistoryRepo;
        }

        [HttpGet]
        [Route("requests")]
        public IHttpActionResult GetRequests()
        {
            var Requests = _requestHistoryRepo.Get<RequestHistoryDto>(new Specification<RequestHistory>(s => s.RequestStatus == RequestStatus.pending));
            return Ok(Requests);
        }
        [HttpGet]
        [Route("request/{id:long}")]
        public IHttpActionResult GetRequestDetailsById(long id)
        {
            var request = _requestHistoryRepo.First<RequestHistoryDto>(new Specification<RequestHistory>(s => s.RequestId == id && s.RequestStatus != RequestStatus.pending));
            return Ok(request);
        }

        [HttpPost]
        [Route("assignRequest/{requestId:long}/{superAdminId:long}")]
        public IHttpActionResult AssignRequest(long requestId)
        {
            // chnage when update vendor to latest vendor
            return Ok();
        }
    }
}
