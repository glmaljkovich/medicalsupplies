using System.Collections.Generic;

namespace ArqNetCore.DTOs.Auth
{
    public class AuthVerifyDTO
    {
        public string ValueRaw { get; set; }   

        public byte[] ValueHash { get; set; }

        public byte[] ValueSalt { get; set; }
    }
}