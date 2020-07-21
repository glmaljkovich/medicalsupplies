using System.Collections.Generic;


namespace ArqNetCore.DTOs.SuppliesOrder
{
    public class SuppliesOrderListResultDTO
    {
        public IEnumerable<SuppliesOrderListItemResultDTO> Items { get; set; }

    }

    public class SuppliesOrderListItemResultDTO
    {
        public int Id { get; set; }
        public string AreaId { get; set; }
        public int? OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string SupplyType { get; set; }
        public string InformerId { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }

    }
}