using System;

namespace ArqNetCore.Exceptions
{
    public class AccountAlreadyExistsException : Exception
    {
        public AccountAlreadyExistsException()
        : base("There's an account already registered for that email address")
        {
        }
    }
}