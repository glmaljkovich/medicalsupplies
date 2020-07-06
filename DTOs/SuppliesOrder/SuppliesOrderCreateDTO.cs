using System.Collections.Generic;

namespace ArqNetCore.DTOs.SuppliesOrder
{
    public class SuppliesOrderCreateDTO
    {
        public string SupplyType { get; set; }
        public IDictionary<string, string> SupplyAttributes { get; set; }
        public string AreaId { get; set; }

    }
}