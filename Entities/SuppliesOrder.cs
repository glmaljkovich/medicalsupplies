using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArqNetCore.Entities

{
    public class SuppliesOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
        [StringLength(32)]
        public string AreaId { get; set; }
        [ForeignKey("AreaId")]
        public virtual Area Area { get; set; }
        public int? OrganizationId { get; set; }
        [ForeignKey("OrganizationId")]
        public virtual Organization Organization { get; set; }

        [StringLength(32)]
        public string Status { get; set; }

    }

    public class SuppliesOrderStatus{
        public static string ACCEPTED { get; set; } = "ACCEPTED";
        public static string PENDING { get; set; } = "PENDING";
        public static string REJECTED { get; set; } = "REJECTED";
    }
}