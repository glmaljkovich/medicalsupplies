using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArqNetCore.Entities

{
    public class OrganizationSupplyType
    {
        
        public int OrganizationId { get; set; }
        [ForeignKey("OrganizationId")]
        public virtual Organization Organization { get; set; }
        [StringLength(32)]
        public string SupplyTypeId {get; set;}
        [ForeignKey("SupplyTypeId")] 
        public virtual SupplyType SupplyType {get; set;}

    }
}