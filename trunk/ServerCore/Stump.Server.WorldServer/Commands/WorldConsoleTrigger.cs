
using System;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Messages.Framework.IO;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;

namespace Stump.Server.WorldServer.Commands
{
    public class WorldConsoleTrigger : TriggerBase
    {
        public WorldConsoleTrigger(StringStream args)
            : base(args, RoleEnum.Administrator)
        {
        }

        public WorldConsoleTrigger(string args)
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