using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArqNetCore.Entities

{
    public class SuppliesOrder
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [ForeignKey("Id")]
        public Account Account;
        [ForeignKey("Id")]
        public Area Area;

    }
}