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
using System.Collections.Generic;
using System.Drawing;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.World.Zones;

namespace Stump.Server.WorldServer.Commands
{
    public class GoPosCommand : WorldCommand
    {
        public GoPosCommand()
        {
            Aliases = new[] {"gopos"};
            RequiredRole = RoleEnum.Moderator;
            Description = "Teleport the target given map position (x/y)";
            Parameters = new List<ICommandParameter>
                {
                    new CommandParameter<Character>("target", "t", "Target to teleport",
                                                    converter: ParametersConverter.CharacterConverter),
                    new CommandParameter<int>("x"),
                    new CommandParameter<int>("y"),
                    new CommandParameter<int>("continent", "c", "Continent containing the map", true),
                };
        }

        public override void Execute(TriggerBase trigger)
        {
            var point = new Point(trigger.GetArgument<int>("x"), trigger.GetArgument<int>("y"));
            var target = trigger.GetArgument<Character>("target");

            SuperArea continent = trigger.ArgumentExists("continent")
                                      ? World.Instance.Continents[trigger.GetArgument<int>("continent")]
                                      : target.Continent;

            if (!continent.MapsByPosition.ContainsKey(point))
            {
                trigger.Reply("Map x:{0} y:{0} doesn't exists or is indoor", point.X, point.Y);
            }
            else
            {
                target.ChangeMap(continent.MapsByPosition[point]);

                trigger.Reply("Teleported.");
            }
        }
    }
}