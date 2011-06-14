using System;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Handlers;
using Stump.Server.WorldServer.World.Actors.Character;
using Stump.Server.WorldServer.World.Entities.Characters;

namespace Stump.Server.WorldServer.Commands
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
    }
}