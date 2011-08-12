using Stump.Core.IO;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Handlers;
using Stump.Server.WorldServer.Handlers.Authorized;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Commands.Trigger
{
    public class TriggerConsole : TriggerBase, IInGameTrigger
    {
        public TriggerConsole(StringStream args, Character character)
            : base(args, character == null ? RoleEnum.Administrator : character.Client.Account.Role)
        {
            Character = character;
        }

        public TriggerConsole(string args, Character character)
            : base(args, character == null ? RoleEnum.Administrator : character.Client.Account.Role)
        {
            Character = character;
        }

        public Character Character
        {
            get;
            private set;
        }

        public override void Reply(string text)
        {
            AuthorizedHandler.SendConsoleMessage(Character.Client, text);
        }

        public override void ReplyError(string message)
        {
            AuthorizedHandler.SendConsoleMessage(Character.Client, ConsoleMessageTypeEnum.CONSOLE_ERR_MESSAGE, message);
        }
    }
}