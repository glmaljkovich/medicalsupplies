using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using ArqNetCore.Services;
using ArqNetCore.DTOs.Area;

namespace ArqNetCore.Controllers
{
    [Authorize]
    [ApiController]
    [Route("areas")]
    public class AreaOrderController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private IMapper _mapper;
        private IAreaService _iAreaService;
        
        public AreaOrderController(
            ILogger<UserController> logger,
            IMapper mapper,
            IAreaService iAreaService
        )
        {
            _logger = logger;
            _mapper = mapper;
            _iAreaService = iAreaService;
        }

        
        [Authorize]
        [HttpGet]
        [Route("")]
        public AreaListResponseDTO List()
        { 
            _logger.LogInformation("List areas:");
            AreaListResultDTO areaListResultDTO = _iAreaService.List();
            AreaListResponseDTO areaListResponseDTO = new AreaListResponseDTO
            {
                Items = areaListResultDTO.Items.Select(
                    (AreaListItemResultDTO areaListItemResultDTO) => {
                    return new AreaListItemResponseDTO
                    {
                        Name = areaListItemResultDTO.Name,
                        Description = areaListItemResultDTO.Description
                    };
                }).ToList()
            };
            return areaListResponseDTO;
        }

    }
}
