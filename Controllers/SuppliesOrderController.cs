using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using ArqNetCore.Services;
using ArqNetCore.DTOs.SuppliesOrder;
using System.Collections.Generic;
namespace ArqNetCore.Controllers
{
    [Authorize]
    [ApiController]
    [Route("supplies-orders")]
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
                SupplyAttributes = suppliesOrderCreateRequestDTO.Supply_attributes,
                AreaId = suppliesOrderCreateRequestDTO.Area_id
            };
            SuppliesOrderCreateResultDTO suppliesOrderCreateResultDTO = _iSuppliesOrderService.Create(suppliesOrderCreateDTO);
            SuppliesOrderCreateResponseDTO suppliesOrderCreateResponseDTO = new SuppliesOrderCreateResponseDTO
            {
                Id = suppliesOrderCreateResultDTO.Id
            };
            return suppliesOrderCreateResponseDTO;
        }
        
        [Authorize]
        [HttpGet]
        [Route("")]
        public SuppliesOrderListResponseDTO List([FromQuery(Name = "informer_id")] string informer_id)
        { 
            _logger.LogInformation("List supplies orders:");
            SuppliesOrderListResultDTO suppliesOrderListResultDTO = _iSuppliesOrderService.List();
            List<SuppliesOrderListItemResponseDTO> FilteredItems = suppliesOrderListResultDTO.Items.Select(
                (SuppliesOrderListItemResultDTO suppliesOrderListItemResultDTO) => {
                    return new SuppliesOrderListItemResponseDTO
                    {
                        Id = suppliesOrderListItemResultDTO.Id,
                        Supply_type = suppliesOrderListItemResultDTO.SupplyType,
                        Informer_id = suppliesOrderListItemResultDTO.InformerId,
                        Status = suppliesOrderListItemResultDTO.Status,
                        Organization_id = suppliesOrderListItemResultDTO.OrganizationId,
                        Organization_name = suppliesOrderListItemResultDTO.OrganizationName,
                        Area_id = suppliesOrderListItemResultDTO.AreaId,
                        Note = suppliesOrderListItemResultDTO.Note
                    };
                }).ToList();
                if (informer_id != null) {
                    FilteredItems = FilteredItems.FindAll((SuppliesOrderListItemResponseDTO item) => item.Informer_id == informer_id);
                }
            SuppliesOrderListResponseDTO suppliesOrderListResponseDTO = new SuppliesOrderListResponseDTO
            {
                Items = FilteredItems
            };
            return suppliesOrderListResponseDTO;
        }

        [Authorize]
        [HttpGet]
        [Route("supply-types")]
        public SuppliesOrderSupplyTypesResponseDTO SupplyTypes()
        {
            SuppliesOrderSupplyTypesResultDTO suppliesOrderSupplyTypesResultDTO = _iSuppliesOrderService.SupplyTypes();
            SuppliesOrderSupplyTypesResponseDTO suppliesOrderSupplyTypesResponseDTO = new SuppliesOrderSupplyTypesResponseDTO
            {
                Items = suppliesOrderSupplyTypesResultDTO.Items
                .Select((SuppliesOrderSupplyTypesItemResultDTO suppliesOrderSupplyTypesItemResultDTO) => new SuppliesOrderSupplyTypesItemResponseDTO
                {
                    Id = suppliesOrderSupplyTypesItemResultDTO.Id,
                    Description = suppliesOrderSupplyTypesItemResultDTO.Description,
                    Supply_attributes = suppliesOrderSupplyTypesItemResultDTO.SupplyAttributes
                }).ToList()
            };
            return suppliesOrderSupplyTypesResponseDTO;
        }

        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public SuppliesOrderGetByIdResponseDTO GetById(int id)
        { 
            _logger.LogInformation("accept supplies order");
            SuppliesOrderGetByIdResultDTO suppliesOrderGetByIdResultDTO = _iSuppliesOrderService.GetById(id);
            SuppliesOrderGetByIdResponseDTO suppliesOrderGetByIdResponseDTO = new SuppliesOrderGetByIdResponseDTO{ 
                Id = suppliesOrderGetByIdResultDTO.Id,
                Supply_type = suppliesOrderGetByIdResultDTO.SupplyType,
                Area_id = suppliesOrderGetByIdResultDTO.AreaId,
                Status = suppliesOrderGetByIdResultDTO.Status,
                Informer_id = suppliesOrderGetByIdResultDTO.InformerId,
                Organization_id = suppliesOrderGetByIdResultDTO.OrganizationId,
                Organization_name = suppliesOrderGetByIdResultDTO.OrganizationName,
                Supply_attributes = suppliesOrderGetByIdResultDTO.SupplyAttributes
            };
            return suppliesOrderGetByIdResponseDTO;
        }

        [Authorize]
        [HttpDelete]
        [Route("{id}")]
        public SuppliesOrderRemoveResponseDTO Remove(int id)
        { 
            _logger.LogInformation("accept supplies order");
            SuppliesOrderRemoveResultDTO suppliesOrderGetByIdResultDTO = _iSuppliesOrderService.Remove(id);
            SuppliesOrderRemoveResponseDTO suppliesOrderGetByIdResponseDTO = new SuppliesOrderRemoveResponseDTO{ 
                Id = suppliesOrderGetByIdResultDTO.Id
            };
            return suppliesOrderGetByIdResponseDTO;
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
                SuppliesOrderId = suppliesOrderRejectRequestDTO.Supplies_order_id,
                Note = suppliesOrderRejectRequestDTO.Note
            };
            SuppliesOrderRejectResultDTO suppliesOrderCreateResultDTO = _iSuppliesOrderService.Reject(suppliesOrderRejectDTO);
            SuppliesOrderRejectResponseDTO suppliesOrderRejectResponseDTO = new SuppliesOrderRejectResponseDTO{ };
            return suppliesOrderRejectResponseDTO;
        }
    }
}
