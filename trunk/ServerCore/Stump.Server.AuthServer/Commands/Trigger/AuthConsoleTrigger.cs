using System;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;

namespace Stump.Server.AuthServer.Commands.Trigger
{
    public class AuthConsoleTrigger : TriggerBase
    {
        public AuthConsoleTrigger(StringStream args)
            : base(args, RoleEnum.Administrator)
        {
        }

        public AuthConsoleTrigger(string args)
            : base(args, RoleEnum.Administrator)
        {
        }

        public override void Reply(string text)
        {
            string name = BindedCommand.Aliases.FirstOrDefault();

            if (!string.IsNullOrEmpty(name))
                Console.WriteLine(name + " : " + text);
            else
                Console.WriteLine(text);
        }
    }
}