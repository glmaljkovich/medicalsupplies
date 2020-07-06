using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArqNetCore.Entities

{
    public class SupplyType {
        [Key]
        [StringLength(32)]
        public string Id { get; set; }

        public string Description { get; set; }
    }
}