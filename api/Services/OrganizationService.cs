using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ArqNetCore.DTOs.Organization;
using ArqNetCore.Entities;
using ArqNetCore.Configuration;
using System.Linq;

namespace ArqNetCore.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly ILogger<OrganizationService> _logger;
        private ArqNetCoreDbContext _dbContext;
        private IUserService _iUserService;

        public OrganizationService(
            ILogger<OrganizationService> logger,
            ArqNetCoreDbContext dbContext,
            IUserService iUserService
        )
        {
            _logger = logger;
            _dbContext = dbContext;
            _iUserService = iUserService;
        }

        public OrganizationListResultDTO List(){
            DbSet<Organization> organizations = _dbContext.Organizations;
            IEnumerable<OrganizationListItemResultDTO> items = organizations.Select((Organization organization) => new OrganizationListItemResultDTO{
                Id = organization.Id,
                Name = organization.Name
            });
            return new OrganizationListResultDTO{
                items = items
            };
        }
    }
}