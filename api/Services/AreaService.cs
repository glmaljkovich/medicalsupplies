using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using ArqNetCore.DTOs.Area;
using ArqNetCore.Entities;
using ArqNetCore.Configuration;
using System.Linq;

namespace ArqNetCore.Services
{
    public class AreaService : IAreaService
    {
        private readonly ILogger<UserService> _logger;
        private ArqNetCoreDbContext _dbContext;
        private IUserService _iUserService;

        public AreaService(
            ILogger<UserService> logger,
            ArqNetCoreDbContext dbContext,
            IUserService iUserService
        )
        {
            _logger = logger;
            _dbContext = dbContext;
            _iUserService = iUserService;
        }


        public AreaListResultDTO List(){
            DbSet<Area> areas = _dbContext.Areas;
            IEnumerable<AreaListItemResultDTO> items = areas
            .Select((Area area) => new AreaListItemResultDTO
            {
                Name = area.Name,
                Description = area.Description,
            });
            return new AreaListResultDTO{
                Items = items
            };
        }


    }
}