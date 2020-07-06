using System.Collections.Generic;


namespace ArqNetCore.DTOs.Organization
{
    public class OrganizationGroupBySupplyTypeResultDTO
    {
        public IEnumerable<OrganizationGroupBySupplyTypeItemResultDTO> Items { get; set; }

    }

    public class OrganizationGroupBySupplyTypeItemResultDTO
    {
        public string SupplyTypeId { get; set; }
        public IEnumerable<OrganizationGroupBySupplyTypeItemOrganizationResultDTO> Organizations { get; set; }
    
    }

    public class OrganizationGroupBySupplyTypeItemOrganizationResultDTO
    {
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }

    }
}