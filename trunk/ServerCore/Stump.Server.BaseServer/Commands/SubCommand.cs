
using System;

namespace Stump.Server.BaseServer.Commands
{
    public abstract class SubCommand : ExecuterBase
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