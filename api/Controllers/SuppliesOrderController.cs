using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    [Route("supply-order")]
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
            SuppliesOrderCreateDTO suppliesOrderCreateDTO = new SuppliesOrderCreateDTO{
                SupplyType = suppliesOrderCreateRequestDTO.Supply_type,
                SupplyDescription = suppliesOrderCreateRequestDTO.Supply_description,
                AreaId = suppliesOrderCreateRequestDTO.Area_id
            };
            SuppliesOrderCreateResultDTO suppliesOrderCreateResultDTO = _iSuppliesOrderService.Create(suppliesOrderCreateDTO);
            SuppliesOrderCreateResponseDTO suppliesOrderCreateResponseDTO = new SuppliesOrderCreateResponseDTO{
                Id = suppliesOrderCreateResultDTO.Id
            };
            return null;
        }
    }
}
