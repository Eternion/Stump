using System;
using Stump.Core.IO;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.Network;

namespace Stump.Server.WorldServer.Commands.Trigger
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

        public override bool CanFormat
        {
            get
            {
                return false;
            }
        }

        public override void Reply(string text)
        {
            Console.WriteLine(" " + text);
        }

        public override BaseClient GetSource()
        {
            return null;
        }
    }
}