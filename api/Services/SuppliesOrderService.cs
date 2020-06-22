using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

            DbSet<SupplyType>          supplyTypes = _dbContext.SupplyTypes;
            DbSet<SupplyTypeAttribute> supplyTypeAttributes = _dbContext.SupplyTypeAttributes;
            DbSet<SuppliesOrder>       suppliesOrders = _dbContext.SuppliesOrders;
            DbSet<SupplyAttribute>     supplyAttributes = _dbContext.SupplyAttributes;
            DbSet<Account>             accounts = _dbContext.Accounts;
            DbSet<Area>                areas = _dbContext.Areas;

            UserAuthContextDTO userAuthContextDTO = _iUserService.UserAuthContext();
            Account account = accounts.Find(userAuthContextDTO.Id);
            Area area = areas.Find(supplyOrderCreateDTO.AreaId);
            //TODO verify not null area
            SuppliesOrder suppliesOrder = new SuppliesOrder
            {
                Account = account,
                Area = area
            };
            SupplyType supplyType = supplyTypes.Find(supplyOrderCreateDTO.SupplyType);
            EntityEntry<SuppliesOrder> entityEntry = suppliesOrders.Add(suppliesOrder);
            Supply supply = new Supply
            {
                SupplyType = supplyType,
                SuppliesOrder = suppliesOrder
            };

            IEnumerable<SuppliesOrderCreateAttributeDTO> suppliesOrderCreateAttributeDTOs = supplyOrderCreateDTO.SupplyAttributes;
            foreach (SuppliesOrderCreateAttributeDTO suppliesOrderCreateAttributeDTO in suppliesOrderCreateAttributeDTOs)
            {
                // SupplyAttribute  * -> Supply              * -> SupplyType
                //                  * -> SupplyTypeAttribute * -> SupplyType
                //TODO verify not null supplyType
                string attributeName = suppliesOrderCreateAttributeDTO.SupplyAttributeName;
                SupplyTypeAttribute supplyTypeAttribute = supplyTypeAttributes.Find(supplyType.Id, attributeName);
                //TODO verify not null supplyTypeAttribute
                SupplyAttribute supplyAttribute = new SupplyAttribute{
                    Supply = supply,
                    AttributeName = attributeName,
                    AttributeValue = suppliesOrderCreateAttributeDTO.SupplyAttributeValue
                };
                supplyAttributes.Add(supplyAttribute);
            }
            _dbContext.Supplies.Add(supply);
            _dbContext.SaveChanges();
            return new SuppliesOrderCreateResultDTO
            {
                Id = suppliesOrder.Id
            };
        }
    }
}