using System;

namespace ArqNetCore.Exceptions.Auth
{
    public class AuthVerifyFailException : Exception
    {
        public AuthVerifyFailException()
        : base("Hashing verification failed")
        {
        }
    }
}