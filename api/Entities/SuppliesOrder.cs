using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArqNetCore.Entities

{
    public class SuppliesOrder
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        
        [ForeignKey("Id")]
        public Supply Supply;

        [ForeignKey("Email")]
        public User User;
        [ForeignKey("Id")]
        public Area Area;

    }
}