
using System;
using Stump.Server.BaseServer.Commands.Commands;

namespace Stump.Server.BaseServer.Commands
{
    public abstract class SubCommand : CommandBase
    {
        public Type ParentCommand
        {
            get;
            protected set;
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}