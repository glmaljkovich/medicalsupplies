using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ArqNetCore.DTOs.SuppliesOrder;
using ArqNetCore.DTOs.User;
using ArqNetCore.Entities;
using ArqNetCore.Configuration;
using System.Linq;
using System;

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

        public SuppliesOrderGetByIdResultDTO GetById(int id){
            DbSet<Supply> supplies = _dbContext.Supplies;
            Supply supply = supplies
            .Include((Supply supply) => supply.SuppliesOrder)
                .ThenInclude((SuppliesOrder suppliesOrder) => suppliesOrder.Organization)
            .First((Supply supply) => supply.Id == id);
            return new SuppliesOrderGetByIdResultDTO
            {
                Id = supply.SuppliesOrderId,
                SupplyType = supply.SupplyTypeId,
                AreaId = supply.SuppliesOrder.AreaId,
                Status = supply.SuppliesOrder.Status,
                InformerId = supply.SuppliesOrder.AccountId,
                OrganizationId = supply.SuppliesOrder.OrganizationId,
                OrganizationName = supply.SuppliesOrder.Organization != null ? supply.SuppliesOrder.Organization.Name : null,
                SupplyAttributes = _dbContext.SupplyAttributes
                .Where((SupplyAttribute supplyAttribute) => supplyAttribute.Supply == supply)
                .ToDictionary(
                    (SupplyAttribute supplyAttribute) => supplyAttribute.AttributeName,
                    (SupplyAttribute supplyAttribute) => supplyAttribute.AttributeValue
                )
            };
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
                Status = SuppliesOrderStatus.PENDING
            };
            SupplyType supplyType = supplyTypes.Find(supplyOrderCreateDTO.SupplyType);
            //TODO verify not null supplyType
            EntityEntry<SuppliesOrder> entityEntry = suppliesOrders.Add(suppliesOrder);
            Supply supply = new Supply
            {
                SupplyType = supplyType,
                SuppliesOrder = suppliesOrder
            };

            IDictionary<string, string> suppliesOrderCreateAttributeDTOs = supplyOrderCreateDTO.SupplyAttributes;
            foreach (KeyValuePair<string, string> suppliesOrderCreateAttributeDTO in suppliesOrderCreateAttributeDTOs)
            {
                // SupplyAttribute  * -> Supply              * -> SupplyType
                //                  * -> SupplyTypeAttribute * -> SupplyType
                string attributeName = suppliesOrderCreateAttributeDTO.Key;
                SupplyTypeAttribute supplyTypeAttribute = supplyTypeAttributes.Find(supplyType.Id, attributeName);
                //TODO verify not null supplyTypeAttribute
                SupplyAttribute supplyAttribute = new SupplyAttribute{
                    Supply = supply,
                    AttributeName = attributeName,
                    AttributeValue = suppliesOrderCreateAttributeDTO.Value
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
            IEnumerable<SuppliesOrderListItemResultDTO> items = supplies
            .Select((Supply supply) => new SuppliesOrderListItemResultDTO
            {
                Id = supply.SuppliesOrder.Id,
                SupplyType = supply.SupplyTypeId,
                AreaId = supply.SuppliesOrder.AreaId,
                Status = supply.SuppliesOrder.Status,
                InformerId = supply.SuppliesOrder.AccountId,
                OrganizationId = supply.SuppliesOrder.OrganizationId,
                OrganizationName = supply.SuppliesOrder.Organization != null ? supply.SuppliesOrder.Organization.Name : null
            });
            return new SuppliesOrderListResultDTO{
                Items = items
            };
        }


        // var res = ctx.Parents.Select(
        //   p => new Dictionary<int, int>(
        //     p.Children.Select(
        //       c => new KeyValuePair<int, int>(
        //         c.Id, 
        //         c.ParentId
        //       )
        //     )
        //   )
        // ).ToArray();

        // var res = ctx.Parents.Select(
        //   p => p.Children.ToDictionary(
        //     c => c.Id, 
        //     c => c.ParentId
        //   )
        // ).ToArray();
        public SuppliesOrderSupplyTypesResultDTO SupplyTypes()
        {
            return new SuppliesOrderSupplyTypesResultDTO
            {
                Items = _dbContext.SupplyTypes
                .Select((SupplyType supplyType)=> new SuppliesOrderSupplyTypesItemResultDTO
                {
                    Id = supplyType.Id,
                    Description = supplyType.Description,
                    SupplyAttributes = new Dictionary<string, string>(
                        _dbContext.SupplyTypeAttributes
                        .Where((SupplyTypeAttribute supplyTypeAttribute) => supplyTypeAttribute.SupplyType == supplyType)
                        .Select((SupplyTypeAttribute supplyTypeAttribute) => new KeyValuePair<string, string>(
                            supplyTypeAttribute.AttributeName,
                            supplyTypeAttribute.AttributeDescription
                        ))
                    )
                    // .ToDictionary(
                    //     (SupplyTypeAttribute supplyTypeAttribute) => supplyTypeAttribute.AttributeName,
                    //     (SupplyTypeAttribute supplyTypeAttribute) => supplyTypeAttribute.AttributeDescription
                    // )
                })
            };
        }
        public SuppliesOrderAcceptResultDTO Accept(SuppliesOrderAcceptDTO suppliesOrderAcceptDTO){
            DbSet<SuppliesOrder> suppliesOrders = _dbContext.SuppliesOrders;
            DbSet<Organization> organizations = _dbContext.Organizations;
            SuppliesOrder suppliesOrder = suppliesOrders.Find(suppliesOrderAcceptDTO.SuppliesOrderId);
            Organization organization = organizations.Find(suppliesOrderAcceptDTO.OrganizationId);
            suppliesOrder.Status = SuppliesOrderStatus.ACCEPTED;
            suppliesOrder.Organization = organization;
            suppliesOrders.Update(suppliesOrder);
            _dbContext.SaveChanges();
            return new SuppliesOrderAcceptResultDTO{ };
        }

        public SuppliesOrderRejectResultDTO Reject(SuppliesOrderRejectDTO suppliesOrderRejectDTO){
            DbSet<SuppliesOrder> suppliesOrders = _dbContext.SuppliesOrders;
            SuppliesOrder suppliesOrder = suppliesOrders.Find(suppliesOrderRejectDTO.SuppliesOrderId);
            suppliesOrder.Status = SuppliesOrderStatus.REJECTED;
            suppliesOrders.Update(suppliesOrder);
            _dbContext.SaveChanges();
            return new SuppliesOrderRejectResultDTO{ };
        }
    }
}