namespace ArqNetCore.DTOs.User
{
    public class UserProfileResultDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Company { get; set; }
        public string Position { get; set; }
        public string Locality { get; set; }
        public string Email { get; set; }

        public bool IsAdmin { get; set; }
    }
}