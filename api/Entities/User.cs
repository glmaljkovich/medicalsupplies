using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArqNetCore.Entities
{
    public class User
    {
        [Key]
        public string Email { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Company { get; set; }

        public string Position { get; set; }
        
        public string Locality { get; set; }

        [ForeignKey("Email")]
        public virtual Account Account { get; set; }
    }
}