using System.ComponentModel.DataAnnotations;

namespace ArqNetCore.Entities
{
    public class Account
    {
        [Key]
        public string Id { get; set; }
        
        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public bool Enable { get; set; }
    }
}