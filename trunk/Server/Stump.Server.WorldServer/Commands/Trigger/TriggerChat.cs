using Stump.Core.IO;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;

namespace Stump.Server.WorldServer.Commands.Trigger
{
    public class TriggerChat : TriggerBase, IInGameTrigger
    {
        public TriggerChat(StringStream args, Character character)
            : base(args, character == null ? RoleEnum.Administrator : character.Client.Account.Role)
        {
            Character = character;
        }

        public TriggerChat(StringStream args, RoleEnum role)
            : base(args, role)
        {
            Character = null;
        }

        public TriggerChat(string args, Character character)
            : base(args, character == null ? RoleEnum.Administrator : character.Client.Account.Role)
        {
            Character = character;
        }

        public TriggerChat(string args, RoleEnum role)
            : base(args, role)
        {
            Character = null;
        }

        public Character Character
        {
            get;
            private set;
        }

        public override void Reply(string text)
        {
            ChatHandler.SendChatServerMessage(Character.Client, text);
        }
    }
}