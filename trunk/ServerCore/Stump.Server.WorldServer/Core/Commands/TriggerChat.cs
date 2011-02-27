// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Handlers;
using Stump.Server.WorldServer.World.Actors.Character;
using Stump.Server.WorldServer.World.Entities.Characters;

namespace Stump.Server.WorldServer.Commands
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