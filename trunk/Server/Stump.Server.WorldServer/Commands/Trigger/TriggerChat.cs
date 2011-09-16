using System;
using Stump.Core.IO;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Handlers.Chat;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Commands.Trigger
{
    public class TriggerChat : GameTrigger
    {
        public TriggerChat(StringStream args, Character character)
            : base(args, character == null ? RoleEnum.Administrator : character.Client.Account.Role, character)
        {
        }

        public TriggerChat(string args, Character character)
            : base(args, character == null ? RoleEnum.Administrator : character.Client.Account.Role, character)
        {
        }

        public override void Reply(string text)
        {
            ChatHandler.SendChatServerMessage(Character.Client, text);
        }

        public override BaseClient GetSource()
        {
            return Character != null ? Character.Client : null;
        }
    }
}