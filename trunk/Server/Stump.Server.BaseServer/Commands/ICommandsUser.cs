using System;
using System.Collections.Generic;

namespace Stump.Server.BaseServer.Commands
{
    public interface ICommandsUser
    {
        List<KeyValuePair<string, Exception>> CommandsErrors
        {
            get;
        }
    }
}