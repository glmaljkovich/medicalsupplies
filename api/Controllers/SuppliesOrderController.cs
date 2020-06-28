using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using ArqNetCore.Services;
using ArqNetCore.DTOs.SuppliesOrder;

namespace ArqNetCore.Controllers
{
    [Authorize]
    [ApiController]
    [Route("supplies-order")]
    public class SuppliesOrderOrderController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private IMapper _mapper;
        private ISuppliesOrderService _iSuppliesOrderService;
        
        public SuppliesOrderOrderController(
            ILogger<UserController> logger,
            IMapper mapper,
            ISuppliesOrderService iSuppliesOrderService
        )
        {
            _logger = logger;
            _mapper = mapper;
            _iSuppliesOrderService = iSuppliesOrderService;
        }

        [Authorize]
        [HttpPost]
        [Route("")]
        public SuppliesOrderCreateResponseDTO Create(SuppliesOrderCreateRequestDTO suppliesOrderCreateRequestDTO)
        { 
            _logger.LogInformation("Create supplies order:" + suppliesOrderCreateRequestDTO.Supply_type);
            SuppliesOrderCreateDTO suppliesOrderCreateDTO = new SuppliesOrderCreateDTO
            {
                SupplyType = suppliesOrderCreateRequestDTO.Supply_type,
                SupplyAttributes = _map(suppliesOrderCreateRequestDTO.Supply_attributes),
                AreaId = suppliesOrderCreateRequestDTO.Area_id
            };
            SuppliesOrderCreateResultDTO suppliesOrderCreateResultDTO = _iSuppliesOrderService.Create(suppliesOrderCreateDTO);
            SuppliesOrderCreateResponseDTO suppliesOrderCreateResponseDTO = new SuppliesOrderCreateResponseDTO
            {
                Id = suppliesOrderCreateResultDTO.Id
            };
            return suppliesOrderCreateResponseDTO;
        }
        
        private IEnumerable<SuppliesOrderCreateAttributeDTO> _map(IEnumerable<Supply_attributes> suppliesOrderCreateRequestDTOs){
            if(suppliesOrderCreateRequestDTOs == null){
                return new List<SuppliesOrderCreateAttributeDTO>();
            }
            return suppliesOrderCreateRequestDTOs.Select((Supply_attributes supplyAttributes) => 
            {
                return new SuppliesOrderCreateAttributeDTO
                {
                    SupplyAttributeName = supplyAttributes.Name,
                    SupplyAttributeValue = supplyAttributes.Value
                };
            });
        }

        [Authorize]
        [HttpGet]
        [Route("")]
        public SuppliesOrderListResponseDTO List()
        { 
            _logger.LogInformation("List supplies orders:");
            SuppliesOrderListResultDTO suppliesOrderCreateResultDTO = _iSuppliesOrderService.List();
            SuppliesOrderListResponseDTO suppliesOrderCreateResponseDTO = new SuppliesOrderListResponseDTO
            {
                Items = suppliesOrderCreateResultDTO.items.Select(
                    (SuppliesOrderListItemResultDTO x) => {
                    return new SuppliesOrderListItemResponseDTO
                    {
                        Id = x.Id,
                        Organization_id = x.OrganizationId,
                        Area_id = x.AreaId
                    };
                }).ToList()
            };
            return suppliesOrderCreateResponseDTO;
        }

        [Authorize]
        [HttpPost]
        [Route("accept")]
        public SuppliesOrderAcceptResponseDTO Accept(SuppliesOrderAcceptRequestDTO suppliesOrderAcceptRequestDTO)
        { 
            _logger.LogInformation("accept supplies order");
            SuppliesOrderAcceptDTO suppliesOrderAcceptDTO = new SuppliesOrderAcceptDTO{
                SuppliesOrderId = suppliesOrderAcceptRequestDTO.Supplies_order_id,
                OrganizationId = suppliesOrderAcceptRequestDTO.Organization_id
            };
            SuppliesOrderAcceptResultDTO suppliesOrderCreateResultDTO = _iSuppliesOrderService.Accept(suppliesOrderAcceptDTO);
            SuppliesOrderAcceptResponseDTO suppliesOrderAcceptResponseDTO = new SuppliesOrderAcceptResponseDTO{ };
            return suppliesOrderAcceptResponseDTO;
        }

        [Authorize]
        [HttpPost]
        [Route("reject")]
        public SuppliesOrderRejectResponseDTO Reject(SuppliesOrderRejectRequestDTO suppliesOrderRejectRequestDTO)
        { 
            _logger.LogInformation("reject supplies order");
            SuppliesOrderRejectDTO suppliesOrderRejectDTO = new SuppliesOrderRejectDTO{
                SuppliesOrderId = suppliesOrderRejectRequestDTO.Supplies_order_id
            };
            SuppliesOrderRejectResultDTO suppliesOrderCreateResultDTO = _iSuppliesOrderService.Reject(suppliesOrderRejectDTO);
            SuppliesOrderRejectResponseDTO suppliesOrderRejectResponseDTO = new SuppliesOrderRejectResponseDTO{ };
            return suppliesOrderRejectResponseDTO;
        }
    }
}
