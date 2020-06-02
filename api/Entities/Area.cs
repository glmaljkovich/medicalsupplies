using System.ComponentModel.DataAnnotations;

namespace ArqNetCore.Entities

{
    public class Area
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }

    }
}