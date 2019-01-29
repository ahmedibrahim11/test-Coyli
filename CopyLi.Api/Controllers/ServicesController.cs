using CopyLi.Api.Dto;
using CopyLi.Data.Entities.Service;
using Framework.Data.EF;
using System.Web.Http;

namespace CopyLi.Api.Controllers
{
    [RoutePrefix("services")]
    public class ServicesController : ApiController
    {
        #region Fields

        private readonly IRepository<Data.Entities.Service.Services> _servicesRepository;
        private readonly IRepository<ServiceType> _serviceTypesRepository;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        #endregion Fields

        #region Constructors
        public ServicesController(
            IRepository<Data.Entities.Service.Services> servicesRepository,
            IRepository<ServiceType> serviceTypesRepository,
            IUnitOfWorkFactory unitOfWorkFactory)
        {
            _servicesRepository = servicesRepository;
            _serviceTypesRepository = serviceTypesRepository;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        #endregion Constructors

        [Route("")]
        [HttpGet]
        public ServicesDto Get()
        {
            var services = _servicesRepository.GetAll<ServiceDto>();
            var serviceTypes = _serviceTypesRepository.GetAll<ServiceTypeDto>();

            ServicesDto serviceDto = new ServicesDto
            {
                Services = services,
                ServiceTypes = serviceTypes
            };
            return serviceDto;
        }

    }
}
