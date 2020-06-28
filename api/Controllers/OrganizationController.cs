using AutoMapper;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using ArqNetCore.Services;
using ArqNetCore.DTOs.Organization;

namespace ArqNetCore.Controllers
{
    [ApiController]
    [Route("organization")]
    public class OrganizationController : ControllerBase
    {
        private readonly ILogger<OrganizationController> _logger;
        private IMapper _mapper;
        private IUserService _userService;
        private IOrganizationService _iOrganizationService;

        public OrganizationController(
            ILogger<OrganizationController> logger,
            IMapper mapper,
            IUserService userService,
            IOrganizationService iOrganizationService
        )
        {
            _logger = logger;
            _mapper = mapper;
            _userService = userService;
            _iOrganizationService = iOrganizationService;
        }

        [Authorize]
        [HttpGet]
        [Route("")]
        public OrganizationListResponseDTO List()
        { 
            _logger.LogInformation("List supplies orders:");
            OrganizationListResultDTO organizationCreateResultDTO = _iOrganizationService.List();
            OrganizationListResponseDTO organizationCreateResponseDTO = new OrganizationListResponseDTO
            {
                Items = organizationCreateResultDTO.items.Select(
                    (OrganizationListItemResultDTO organizationListItemResultDTO) => 
                    {
                        return new OrganizationListItemResponseDTO
                        {
                            Id = organizationListItemResultDTO.Id,
                            Name = organizationListItemResultDTO.Name
                        };
                    }
                ).ToList()
            };
            return organizationCreateResponseDTO;
        }

        
    }
}
