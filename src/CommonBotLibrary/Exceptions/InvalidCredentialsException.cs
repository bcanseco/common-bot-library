using System;

namespace CommonBotLibrary.Exceptions
{
    public class InvalidCredentialsException : UnauthorizedAccessException
    {
        public InvalidCredentialsException()
        { }

        public InvalidCredentialsException(string message)
            : base(message)
        { }

        public InvalidCredentialsException(string message, Exception inner)
            : base(message, inner)
        { }
    }
}
