using System.ComponentModel.DataAnnotations;

namespace ArqNetCore.Entities
{
    public class SupplyOption {
        [Key]
        public string Id { get; set; }
        public string SupplyType {get; set;}
        public string Description {get; set;}
    }
}