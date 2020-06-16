using Microsoft.Extensions.Logging;
using ArqNetCore.DTOs.SuppliesOrder;
using ArqNetCore.DTOs.User;
using ArqNetCore.Entities;
using ArqNetCore.Configuration;

namespace ArqNetCore.Services
{
    public class SuppliesOrderService : ISuppliesOrderService
    {
        private readonly ILogger<UserService> _logger;
        private ArqNetCoreDbContext _dbContext;
        private IUserService _iUserService;

        public SuppliesOrderService(
            ILogger<UserService> logger,
            ArqNetCoreDbContext dbContext,
            IUserService iUserService
        )
        {
            _logger = logger;
            _dbContext = dbContext;
            _iUserService = iUserService;
        }

        public SuppliesOrderCreateResultDTO Create(SuppliesOrderCreateDTO supplyOrderCreateDTO){
            UserAuthContextDTO userAuthContextDTO = _iUserService.UserAuthContext();
            Account account = _dbContext.Accounts.Find(userAuthContextDTO.Id);
            Area area = _dbContext.Areas.Find(supplyOrderCreateDTO.AreaId);
            SuppliesOrder suppliesOrder = new SuppliesOrder
            {
                Account = account,
                Area = area
            };
            _dbContext.SuppliesOrders.Add(suppliesOrder);
            Supply supply = new Supply
            {
                SupplyType = supplyOrderCreateDTO.SupplyType,
                Description = supplyOrderCreateDTO.SupplyDescription,
                SuppliesOrder = suppliesOrder
            };
            _dbContext.Supplies.Add(supply);
            return new SuppliesOrderCreateResultDTO
            {
                Id = supply.Id
            };
        }
    }
}