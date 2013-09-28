#region License GNU GPL
// TitleHandler.cs
// 
// Copyright (C) 2013 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion

using System.Linq;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Handlers.Titles
{
    public class TitleHandler : WorldHandlerContainer
    {
        [WorldHandler(TitleSelectRequestMessage.Id)]
        public static void HandleTitleSelectRequestMessage(WorldClient client, TitleSelectRequestMessage message)
        {
            if (message.titleId != 0)
                client.Character.SelectTitle(message.titleId);
            else
                client.Character.ResetTitle();
        }

        [WorldHandler(OrnamentSelectRequestMessage.Id)]
        public static void HandleOrnamentSelectRequestMessage(WorldClient client, OrnamentSelectRequestMessage message)
        {
            if (message.ornamentId != 0)
                client.Character.SelectOrnament(message.ornamentId);
            else
                client.Character.ResetOrnament();
        }

        [WorldHandler(TitlesAndOrnamentsListRequestMessage.Id)]
        public static void HandleTitlesAndOrnamentsListRequestMessage(WorldClient client, TitlesAndOrnamentsListRequestMessage message)
        {
            SendTitlesAndOrnamentsListMessage(client, client.Character);
        }

        public static void SendTitleSelectErrorMessage(IPacketReceiver client)
        {
            client.Send(new TitleSelectErrorMessage());
        }

        public static void SendTitleSelectedMessage(IPacketReceiver client, short title)
        {
            client.Send(new TitleSelectedMessage(title));
        }

        public static void SendOrnamentSelectedMessage(IPacketReceiver client, short ornament)
        {
            client.Send(new OrnamentSelectedMessage(ornament));
        }

        public static void SendTitlesAndOrnamentsListMessage(IPacketReceiver client, Character character)
        {
            client.Send(new TitlesAndOrnamentsListMessage(character.Titles, character.Ornaments,
                                                          (short)
                                                          (character.SelectedTitle.HasValue
                                                               ? character.SelectedTitle.Value
                                                               : 0),
                                                          (short)
                                                          (character.SelectedOrnament.HasValue
                                                               ? character.SelectedOrnament.Value
                                                               : 0)));
        }

        public static void SendTitleGainedMessage(IPacketReceiver client, short title)
        {
            client.Send(new TitleGainedMessage(title));
        }

        public static void SendTitleLostMessage(IPacketReceiver client, short title)
        {
            client.Send(new TitleLostMessage(title));
        }

        public static void SendOrnamentGainedMessage(IPacketReceiver client, short ornament)
        {
            client.Send(new OrnamentGainedMessage(ornament));
        }
    }
}