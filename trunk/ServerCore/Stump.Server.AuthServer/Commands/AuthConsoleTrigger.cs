
using System;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Messages.Framework.IO;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;

namespace Stump.Server.AuthServer.Commands
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
            string name = GetBindedCommandName();

            if (name != "")
                Console.WriteLine(GetBindedCommandName() + " : " + text);
            else
                Console.WriteLine(text);
        }
    }
}