using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using ArqNetCore.Services;
using ArqNetCore.DTOs.User;
using ArqNetCore.DTOs.Account;
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
                SupplyAttributes = map(suppliesOrderCreateRequestDTO.Supply_attributes),
                AreaId = suppliesOrderCreateRequestDTO.Area_id
            };
            SuppliesOrderCreateResultDTO suppliesOrderCreateResultDTO = _iSuppliesOrderService.Create(suppliesOrderCreateDTO);
            SuppliesOrderCreateResponseDTO suppliesOrderCreateResponseDTO = new SuppliesOrderCreateResponseDTO
            {
                Id = suppliesOrderCreateResultDTO.Id
            };
            return suppliesOrderCreateResponseDTO;
        }
        

        protected IEnumerable<SuppliesOrderCreateAttributeDTO> map(IEnumerable<Supply_attributes> suppliesOrderCreateRequestDTOs){
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
        public SuppliesOrderListResponseDTO Create()
        { 
            _logger.LogInformation("List supplies orders:");
            SuppliesOrderListResultDTO suppliesOrderCreateResultDTO = _iSuppliesOrderService.List();
            SuppliesOrderListResponseDTO suppliesOrderCreateResponseDTO = new SuppliesOrderListResponseDTO
            {
                Items = suppliesOrderCreateResultDTO.items.Select(x => new SuppliesOrderListItemResponseDTO{
                    Area_id = x.AreaId
                }).ToList()
            };
            return suppliesOrderCreateResponseDTO;
        }
    }
}
