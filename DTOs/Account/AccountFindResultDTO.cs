namespace ArqNetCore.DTOs.Account
{
    public class AccountFindResultDTO
    {

        public string Id { get; set; }
        
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

    }
}