using System.Collections.Generic;
using System.Drawing;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class GoPosCommand : CommandBase
    {
        public GoPosCommand()
        {
            Aliases = new[] {"gopos", "teleporto"};
            RequiredRole = RoleEnum.Moderator;
            Description = "Teleport the target to the given map position (x/y)";
            AddParameter("target", "t", "Target to teleport",
                                                    converter: ParametersConverter.CharacterConverter);
           AddParameter<int>("x");
           AddParameter<int>("y");
           AddParameter<short>("cellId", "cell", "Cell destination", isOptional: true);
           AddParameter<int>("superArea", "area", "Super area containing the map", isOptional:true);
           AddParameter("outDoor", "out", defaultValue: true);
        }

        public override void Execute(TriggerBase trigger)
        {
            var point = new Point(trigger.Get<int>("x"), trigger.Get<int>("y"));
            var target = trigger.Get<Character>("target");

            var superArea = trigger.IsArgumentDefined("superArea")
                                      ? World.Instance.GetSuperArea(trigger.Get<int>("superArea"))
                                      : target.Map.SubArea.Area.SuperArea;

            if (!superArea.MapsByPosition.ContainsKey(point))
            {
                var map = World.Instance.GetMap(point.X, point.Y, trigger.Get<bool>("out"));

                if (map == null)
                    trigger.ReplyError("Map x:{0} y:{0} doesn't exists or is indoor", point.X, point.Y);

                else
                {
                    var cell = trigger.IsArgumentDefined("cell") ? map.Cells[trigger.Get<short>("cell")] : target.Cell;

                    target.Teleport(new ObjectPosition(map, cell, target.Direction));

                    trigger.Reply("Teleported.");
                }
            }
            else
            {
                var map = superArea.GetMaps(point)[0];
                var cell = trigger.IsArgumentDefined("cell") ? map.Cells[trigger.Get<short>("cell")] : target.Cell;

                target.Teleport(new ObjectPosition(map, cell, target.Direction));

                trigger.Reply("Teleported.");
            }
        }
    }

    public class GoCommand : CommandBase
    {
        public GoCommand()
        {
            Aliases = new[] { "go","teleport", "tp" };
            RequiredRole = RoleEnum.Moderator;
            Description = "Teleport the target given map id";
            AddParameter("target", "t", "Target to teleport",
                                                    converter: ParametersConverter.CharacterConverter);
            AddParameter<int>("mapId", "id", "Map destination");
            AddParameter<short>("cellId", "cell", "Cell destination", isOptional:true);

        }

        public override void Execute(TriggerBase trigger)
        {
            var target = trigger.Get<Character>("target");
            var map = World.Instance.GetMap(trigger.Get<int>("mapId"));

            if (map == null)
            {
                trigger.ReplyError("Map '{0}' doesn't exist", trigger.Get<int>("mapId"));
            }
            else
            {
                var cell = trigger.IsArgumentDefined("cell") ? map.Cells[trigger.Get<short>("cell")] : target.Cell;

                target.Teleport(new ObjectPosition(map, cell, target.Direction));

                trigger.Reply("Teleported.");
            }
        }
    }
}