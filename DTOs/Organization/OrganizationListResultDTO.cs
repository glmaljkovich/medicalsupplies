using System.Collections.Generic;


namespace ArqNetCore.DTOs.Organization
{
    public class OrganizationListResultDTO
    {
        public IEnumerable<OrganizationListItemResultDTO> Items { get; set; }

    }

    public class OrganizationListItemResultDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
}