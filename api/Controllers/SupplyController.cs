using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using ArqNetCore.Services;
using ArqNetCore.DTOs.User;
using ArqNetCore.DTOs.Account;
using ArqNetCore.DTOs.Auth;
using System;

namespace ArqNetCore.Controllers
{
    [Authorize]
    [ApiController]
    [Route("supply")]
    public class SupplyController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private IMapper _mapper;
        
        public SupplyController(
            ILogger<UserController> logger,
            IMapper mapper
        )
        {
            _logger = logger;
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost]
        [Route("")]
        public SuppliesOrderCreateResponseDTO Create(SuppliesOrderCreateRequestDTO suppliesOrderCreateRequestDTO)
        { 
            _logger.LogInformation("Create supplies order:" + suppliesOrderCreateRequestDTO.Supply_type);
            var currentUser = HttpContext.User; 
            return null;
        }
    }
}
