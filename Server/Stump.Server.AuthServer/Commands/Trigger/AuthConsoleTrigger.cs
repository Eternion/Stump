using System;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Enums;
using Stump.Server.AuthServer.IO;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.Network;

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

        public override ICommandsUser User
        {
            get { return AuthServer.Instance.ConsoleInterface as AuthConsole; }
        }

        public override void Log()
        {
        }
    }
}