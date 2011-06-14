
using System;
using System.Runtime.Serialization;

namespace Stump.Server.BaseServer.Commands
{
    public class DummyCommandException : Exception
    {
        public DummyCommandException()
            : this("Cannot execute dummy commands that contains subcommands")
        {
        }

        public DummyCommandException(string message) : base(message)
        {
        }

        public DummyCommandException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DummyCommandException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}