using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArqNetCore.Entities

{
    public class Area
    {
        [Key]
        [StringLength(32)]
        public string Name { get; set; }
        public string Description { get; set; }

    }
}