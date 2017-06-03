using System;
using System.IO;

namespace CommonBotLibrary.Exceptions
{
    public class ResultNotFoundException : FileNotFoundException
    {
        public ResultNotFoundException()
        { }

        public ResultNotFoundException(string message)
            : base(message)
        { }

        public ResultNotFoundException(string message, Exception inner)
            : base(message, inner)
        { }
    }
}
