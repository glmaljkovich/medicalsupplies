using System.Collections.Generic;

namespace ArqNetCore.DTOs.SuppliesOrder
{
    public class SuppliesOrderCreateDTO
    {
        public string SupplyType { get; set; }
        public IEnumerable<SuppliesOrderCreateAttributeDTO> SupplyAttributes { get; set; }
        public string AreaId { get; set; }

    }
    public class SuppliesOrderCreateAttributeDTO
    {
        public string SupplyAttributeName { get; set; }
        public string SupplyAttributeValue { get; set; }
    }
}