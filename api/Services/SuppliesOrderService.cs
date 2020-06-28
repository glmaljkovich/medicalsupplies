using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ArqNetCore.DTOs.SuppliesOrder;
using ArqNetCore.DTOs.User;
using ArqNetCore.Entities;
using ArqNetCore.Configuration;
using System.Linq;

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
            DbSet<SupplyType>          supplyTypes          = _dbContext.SupplyTypes;
            DbSet<SupplyTypeAttribute> supplyTypeAttributes = _dbContext.SupplyTypeAttributes;
            DbSet<Supply>              supplies             = _dbContext.Supplies;
            DbSet<SuppliesOrder>       suppliesOrders       = _dbContext.SuppliesOrders;
            DbSet<SupplyAttribute>     supplyAttributes     = _dbContext.SupplyAttributes;
            DbSet<Account>             accounts             = _dbContext.Accounts;
            DbSet<Area>                areas                = _dbContext.Areas;
            UserAuthContextDTO userAuthContextDTO = _iUserService.UserAuthContext();
            Account account = accounts.Find(userAuthContextDTO.Id);
            Area area = areas.Find(supplyOrderCreateDTO.AreaId);
            //TODO verify not null area
            SuppliesOrder suppliesOrder = new SuppliesOrder
            {
                Account = account,
                Area = area,
                Status = SuppliesOrderStatus.Pending
            };
            SupplyType supplyType = supplyTypes.Find(supplyOrderCreateDTO.SupplyType);
            //TODO verify not null supplyType
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
            supplies.Add(supply);
            _dbContext.SaveChanges();
            return new SuppliesOrderCreateResultDTO
            {
                Id = suppliesOrder.Id
            };
        }

        public SuppliesOrderListResultDTO List(){
            DbSet<Supply> supplies = _dbContext.Supplies;
            IEnumerable<SuppliesOrderListItemResultDTO> items = supplies.Select((Supply supply) => new SuppliesOrderListItemResultDTO{
                Id = supply.SuppliesOrder.Id,
                SupplyType = supply.SupplyTypeId,
                AreaId = supply.SuppliesOrder.AreaId,
                Informer = supply.SuppliesOrder.AccountId
            });
            return new SuppliesOrderListResultDTO{
                items = items
            };
        }

        public SuppliesOrderAcceptResultDTO Accept(SuppliesOrderAcceptDTO suppliesOrderAcceptDTO){
            DbSet<SuppliesOrder> suppliesOrders = _dbContext.SuppliesOrders;
            DbSet<Organization> organizations = _dbContext.Organizations;
            SuppliesOrder suppliesOrder = suppliesOrders.Find(suppliesOrderAcceptDTO.SuppliesOrderId);
            Organization organization = organizations.Find(suppliesOrderAcceptDTO.OrganizationId);
            suppliesOrder.Status = SuppliesOrderStatus.Accepted;
            suppliesOrder.Organization = organization;
            suppliesOrders.Update(suppliesOrder);
            return new SuppliesOrderAcceptResultDTO{ };
        }

        public SuppliesOrderRejectResultDTO Reject(SuppliesOrderRejectDTO suppliesOrderRejectDTO){
            DbSet<SuppliesOrder> suppliesOrders = _dbContext.SuppliesOrders;
            SuppliesOrder suppliesOrder = suppliesOrders.Find(suppliesOrderRejectDTO.SuppliesOrderId);
            suppliesOrder.Status = SuppliesOrderStatus.Rejected;
            suppliesOrders.Update(suppliesOrder);
            return new SuppliesOrderRejectResultDTO{ };
        }
    }
}