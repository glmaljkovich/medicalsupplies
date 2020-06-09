using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArqNetCore.Entities

{
    public class Supply {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string SupplyType {get; set;}
        public string Description {get; set;}
        [ForeignKey("Id")]
        public SuppliesOrder SuppliesOrder;
    }
}