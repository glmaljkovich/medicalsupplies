using System.Collections.Generic;


namespace ArqNetCore.DTOs.SuppliesOrder
{
    public class SuppliesOrderSupplyTypesResultDTO
    {
        public IEnumerable<SuppliesOrderSupplyTypesItemResultDTO> Items { get; set; }

    }

    public class SuppliesOrderSupplyTypesItemResultDTO
    {
        public string Id { get; set; }
        public string Description { get; set; }

        public IDictionary<string, string> SupplyAttributes { get; set; }

    }
}