﻿using System.Collections.Generic;
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
                    (SuppliesOrderListItemResultDTO suppliesOrderListItemResultDTO) => {
                    return new SuppliesOrderListItemResponseDTO
                    {
                        Id = suppliesOrderListItemResultDTO.Id,
                        Supply_type = suppliesOrderListItemResultDTO.SupplyType,
                        Informer_id = suppliesOrderListItemResultDTO.InformerId,
                        Status = suppliesOrderListItemResultDTO.Status,
                        Organization_id = suppliesOrderListItemResultDTO.OrganizationId,
                        Organization_name = suppliesOrderListItemResultDTO.OrganizationName,
                        Area_id = suppliesOrderListItemResultDTO.AreaId
                    };
                }).ToList()
            };
            return suppliesOrderCreateResponseDTO;
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
                Attributes = suppliesOrderGetByIdResultDTO.Attributes
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
                SuppliesOrderId = suppliesOrderRejectRequestDTO.Supplies_order_id
            };
            SuppliesOrderRejectResultDTO suppliesOrderCreateResultDTO = _iSuppliesOrderService.Reject(suppliesOrderRejectDTO);
            SuppliesOrderRejectResponseDTO suppliesOrderRejectResponseDTO = new SuppliesOrderRejectResponseDTO{ };
            return suppliesOrderRejectResponseDTO;
        }
    }
}
