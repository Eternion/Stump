
using System.Collections.Generic;
using System.Drawing;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Global;

namespace Stump.Server.WorldServer.Commands
{
    public class GoPosCommand : WorldCommand
    {
        public GoPosCommand()
        {
            Aliases = new[] {"gopos"};
            RequiredRole = RoleEnum.Moderator;
            Description = "Teleport the target given map position (x/y)";
            Parameters = new List<IParameter>
                {
                    new ParameterDefinition<Character>("target", "t", "Target to teleport",
                                                    converter: ParametersConverter.CharacterConverter),
                    new ParameterDefinition<int>("x"),
                    new ParameterDefinition<int>("y"),
                    new ParameterDefinition<int>("continent", "c", "Continent containing the map", true),
                };
        }

        public override void Execute(TriggerBase trigger)
        {
            var point = new Point(trigger.Get<int>("x"), trigger.Get<int>("y"));
            var target = trigger.Get<Character>("target");

            Continent continent = trigger.IsArgumentDefined("continent")
                                      ? World.Instance.Continents[trigger.Get<int>("continent")]
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