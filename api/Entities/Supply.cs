using System.ComponentModel.DataAnnotations;

namespace ArqNetCore.Entities

{
    public class Supply {
        [Key]
        public string Id { get; set; }
        public string Name {get; set;}
        public string Description {get; set;}

        public bool Proper;
    }
}