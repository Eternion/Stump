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
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Breeds;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class ContextHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof (StatsUpgradeRequestMessage))]
        public static void HandleStatsUpgradeRequestMessage(WorldClient client, StatsUpgradeRequestMessage message)
        {
            var statsid = (CaracteristicsIdEnum) message.statId;

            if (statsid < CaracteristicsIdEnum.Strength ||
                statsid > CaracteristicsIdEnum.Intelligence)
                throw new Exception("Wrong statsid");

            if (message.boostPoint <= 0)
                throw new Exception("Client given 0 as boostpoint. Forbidden value.");

            BaseBreed breed = BreedManager.GetBreed(client.ActiveCharacter.BreedId);
            int neededpts = breed.GetNeededPointForStats(client.ActiveCharacter.Stats[statsid.ToString()].Base, statsid);

            var boost = (int) (message.boostPoint/neededpts);

            if (boost < 0)
                throw new Exception("Client is attempt to use more points that he has.");

            // Exception for Sacrieur Vitality * 2
            if (breed.Id == PlayableBreedEnum.Sacrieur && statsid == CaracteristicsIdEnum.Vitality)
                boost *= 2;

            client.ActiveCharacter.Stats[statsid.ToString()].Base += boost;

            SendStatsUpgradeResultMessage(client, message.boostPoint);
            CharacterHandler.SendCharacterStatsListMessage(client);
        }

        public static void SendStatsUpgradeResultMessage(WorldClient client, uint usedpts)
        {
            client.Send(new StatsUpgradeResultMessage(usedpts));
        }
    }
}