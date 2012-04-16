using System;
using System.Drawing;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class GoPosCommand : TargetCommand
    {
        public GoPosCommand()
        {
            Aliases = new[] {"gopos", "teleporto"};
            RequiredRole = RoleEnum.Moderator;
            Description = "Teleport the target to the given map position (x/y)";
            AddParameter<int>("x");
            AddParameter<int>("y");
            AddTargetParameter(true);
            AddParameter<short>("cellId", "cell", "Cell destination", isOptional: true);
            AddParameter<int>("superArea", "area", "Super area containing the map", isOptional: true);
            AddParameter("outDoor", "out", defaultValue: true);
        }

        public override void Execute(TriggerBase trigger)
        {
            var point = new Point(trigger.Get<int>("x"), trigger.Get<int>("y"));
            var target = GetTarget(trigger);

            SuperArea superArea = trigger.IsArgumentDefined("superArea")
                                      ? World.Instance.GetSuperArea(trigger.Get<int>("superArea"))
                                      : target.Map.SubArea.Area.SuperArea;

            if (!superArea.MapsByPosition.ContainsKey(point))
            {
                Map map = World.Instance.GetMap(point.X, point.Y, trigger.Get<bool>("out"));

                if (map == null)
                    trigger.ReplyError("Map x:{0} y:{0} doesn't exists or is indoor", point.X, point.Y);

                else
                {
                    Cell cell = trigger.IsArgumentDefined("cell") ? map.Cells[trigger.Get<short>("cell")] : target.Cell;

                    target.Teleport(new ObjectPosition(map, cell, target.Direction));

                    trigger.Reply("Teleported.");
                }
            }
            else
            {
                Map map = superArea.GetMaps(point)[0];
                Cell cell = trigger.IsArgumentDefined("cell") ? map.Cells[trigger.Get<short>("cell")] : target.Cell;

                target.Teleport(new ObjectPosition(map, cell, target.Direction));

                trigger.Reply("Teleported.");
            }
        }
    }

    public class GoCommand : TargetCommand
    {
        public GoCommand()
        {
            Aliases = new[] {"go", "teleport", "tp"};
            RequiredRole = RoleEnum.Moderator;
            Description = "Teleport the target given map id";
            AddParameter<int>("mapId", "id", "Map destination");
            AddTargetParameter(true);
            AddParameter<short>("cellId", "cell", "Cell destination", isOptional: true);
        }

        public override void Execute(TriggerBase trigger)
        {
            var target = GetTarget(trigger);
            Map map = World.Instance.GetMap(trigger.Get<int>("mapId"));

            if (map == null)
            {
                trigger.ReplyError("Map '{0}' doesn't exist", trigger.Get<int>("mapId"));
            }
            else
            {
                Cell cell = trigger.IsArgumentDefined("cell") ? map.Cells[trigger.Get<short>("cell")] : target.Cell;

                target.Teleport(new ObjectPosition(map, cell, target.Direction));

                trigger.Reply("Teleported.");
            }
        }
    }

    public class GoNameCommand : CommandBase
    {
        public GoNameCommand()
        {
            Aliases = new[] { "goname", "tptoname" };
            RequiredRole = RoleEnum.Moderator;
            Description = "Teleport to the target";
            AddParameter("to", "to", "The character to rejoin", converter: ParametersConverter.CharacterConverter);
            AddParameter("from", "from", "The character that teleport", isOptional:true, converter: ParametersConverter.CharacterConverter);
        }

        public override void Execute(TriggerBase trigger)
        {
            var to = trigger.Get<Character>("to");
            Character from;

            if (trigger.IsArgumentDefined("from"))
                from = trigger.Get<Character>("from");
            else if (trigger is GameTrigger)
                from = ( trigger as GameTrigger ).Character;
            else
            {
                throw new Exception("Character to teleport not defined !");
            }

            from.Teleport(to.Position);
        }
    }
}