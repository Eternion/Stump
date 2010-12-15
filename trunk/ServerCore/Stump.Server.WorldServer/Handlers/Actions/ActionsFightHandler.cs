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
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class ActionsHandler : WorldHandlerContainer
    {
        public static void SendGameActionFightDeathMessage(WorldClient client, Entity entity)
        {
            client.Send(new GameActionFightDeathMessage(
                            (uint)ActionsEnum.ACTION_CHARACTER_DEATH,
                            (int)entity.Id, (int)entity.Id
                            ));
        }

        public static void SendGameActionFightPointsVariationMessage(WorldClient client, ActionsEnum action, Entity source,
                                                                         Entity target, short delta)
        {
            client.Send(new GameActionFightPointsVariationMessage(
                            (uint)action,
                            (int)source.Id, (int)target.Id, delta
                            ));
        }

        public static void SendGameActionFightLifePointsVariationMessage(WorldClient client, Entity source,
                                                                         Entity target, short delta)
        {
            client.Send(new GameActionFightLifePointsVariationMessage(
                            (uint) ActionsEnum.ACTION_CHARACTER_LIFE_POINTS_LOST,
                            (int) source.Id, (int) target.Id, delta
                            ));
        }
    }
}