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
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class InventoryHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof (SpellMoveMessage))]
        public static void HandleSpellMoveMessage(WorldClient client, SpellMoveMessage message)
        {
            if (message.position < 63 || message.position > 255)
            {
                return;
            }

            client.ActiveCharacter.ModifySpellPos((SpellIdEnum) message.spellId, (int) message.position);

            SendSpellMovementMessage(client, message.spellId, message.position);
        }

        public static void SendSpellMovementMessage(WorldClient client, uint spellId, uint position)
        {
            client.Send(new SpellMovementMessage(spellId, position));
        }

        public static void SendSpellListMessage(WorldClient client, bool previsualization)
        {
            client.Send(new SpellListMessage(previsualization,
                                             client.ActiveCharacter.Spells.SpellsById.Values.Select(
                                                 entry => entry.ToNetworkSpell()).ToList()));
        }
    }
}