using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArqNetCore.Entities

{
    public class SupplyAttribute {
        [Key]
        public int SupplyId {get; set;}
        [ForeignKey("SupplyId")]
        public virtual Supply Supply {get; set;}
        [Key]
        [StringLength(32)]
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
    }
}