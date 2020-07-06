using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArqNetCore.Entities

{
    public class Supply {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string SupplyTypeId {get; set;}
        [ForeignKey("SupplyTypeId")]
        public virtual SupplyType SupplyType {get; set;}
        public int SuppliesOrderId {get; set;}
        [ForeignKey("SuppliesOrderId")]
        public virtual SuppliesOrder SuppliesOrder {get; set;}
    }
}