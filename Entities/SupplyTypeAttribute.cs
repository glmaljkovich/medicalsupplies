using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArqNetCore.Entities

{
    public class SupplyTypeAttribute {
        [StringLength(32)]
        public string SupplyTypeId {get; set;}
        [ForeignKey("SupplyTypeId")] 
        public virtual SupplyType SupplyType {get; set;}
        [StringLength(32)]
        public string AttributeName { get; set; }
        public string AttributeDescription { get; set; }
    }
}