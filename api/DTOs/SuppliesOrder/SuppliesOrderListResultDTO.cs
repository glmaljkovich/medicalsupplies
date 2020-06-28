using System.Collections.Generic;


namespace ArqNetCore.DTOs.SuppliesOrder
{
    public class SuppliesOrderListResultDTO
    {
        public IEnumerable<SuppliesOrderListItemResultDTO> items { get; set; }

    }

    public class SuppliesOrderListItemResultDTO
    {
        public int Id { get; set; }
        public string AreaId { get; set; }
        public string OrganizationId { get; set; }
        public string SupplyType { get; set; }
        public string Informer { get; set; }

    }
}