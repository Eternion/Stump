
using System;
using Stump.Core.IO;
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
            Console.WriteLine(" " + text);
        }
    }
}