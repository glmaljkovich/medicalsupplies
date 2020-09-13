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
    [Route("organizations")]
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
            _logger.LogInformation("List organization:");
            OrganizationListResultDTO organizationListResultDTO = _iOrganizationService.List();
            OrganizationListResponseDTO organizationListResponseDTO = new OrganizationListResponseDTO
            {
                Items = organizationListResultDTO.Items.Select(
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
            return organizationListResponseDTO;
        }

        [Authorize]
        [HttpGet]
        [Route("group-by-supply-type")]
        public OrganizationGroupBySupplyTypeResponseDTO GroupBySupplyType()
        { 
            _logger.LogInformation("organizations group by supply type:");
            OrganizationGroupBySupplyTypeResultDTO organizationGroupBySupplyTypeResultDTO = _iOrganizationService.GroupBySupplyType();
            OrganizationGroupBySupplyTypeResponseDTO organizationGroupBySupplyTypeResponseDTO = new OrganizationGroupBySupplyTypeResponseDTO
            {
                Items = organizationGroupBySupplyTypeResultDTO.Items
                .Select((OrganizationGroupBySupplyTypeItemResultDTO organizationGroupBySupplyTypeItemResultDTO) =>  new OrganizationGroupBySupplyTypeItemResponseDTO
                {
                    Supply_type_id = organizationGroupBySupplyTypeItemResultDTO.SupplyTypeId,
                    Organizations = organizationGroupBySupplyTypeItemResultDTO.Organizations
                    .Select((OrganizationGroupBySupplyTypeItemOrganizationResultDTO organizationGroupBySupplyTypeItemOrganizationResultDTO)=> new OrganizationGroupBySupplyTypeItemOrganizationsResponseDTO{
                        Organization_id = organizationGroupBySupplyTypeItemOrganizationResultDTO.OrganizationId,
                        Organization_name = organizationGroupBySupplyTypeItemOrganizationResultDTO.OrganizationName
                    }).ToList()
                }
                ).ToList()
            };
            return organizationGroupBySupplyTypeResponseDTO;
        }

        
    }
}
