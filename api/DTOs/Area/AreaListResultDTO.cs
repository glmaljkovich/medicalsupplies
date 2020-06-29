using System.Collections.Generic;


namespace ArqNetCore.DTOs.Area
{
    public class AreaListResultDTO
    {
        public IEnumerable<AreaListItemResultDTO> Items { get; set; }

    }

    public class AreaListItemResultDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }

    }
}