using System.Collections.Generic;

namespace ArqNetCore.DTOs.Auth
{
    public class AuthTokenDTO
    {
        public IDictionary<string, string> SubjectRaw;
        public IDictionary<string, object> Claims;
    }
}