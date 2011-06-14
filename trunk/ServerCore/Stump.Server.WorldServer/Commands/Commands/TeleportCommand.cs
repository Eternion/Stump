
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

            Continent continent = trigger.ArgumentExists("continent")
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