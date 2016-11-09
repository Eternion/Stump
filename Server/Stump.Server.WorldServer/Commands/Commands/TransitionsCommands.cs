using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Database.World.Triggers;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Maps.Cells.Triggers;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class TransitionsCommands : SubCommandContainer
    {
        public TransitionsCommands()
        {
            Aliases = new[] {"transition"};
            Description = "Manage map transitions";
            RequiredRole = RoleEnum.Administrator;
        }
         
    }

    public class TransitionSetCommand : SubCommand
    {
        public TransitionSetCommand()
        {
            Aliases = new[] {"set"};
            Description = "Set the current map transition";
            RequiredRole = RoleEnum.Administrator;
            ParentCommandType = typeof (TransitionsCommands);
            AddParameter("transition", "t", "Top/Right/Bottom/Left",
                converter: ParametersConverter.GetEnumConverter<MapNeighbour>());
            AddParameter("map", "m", "The destination", converter: ParametersConverter.MapConverter);
            AddParameter("cell", "c", "The cell destination", isOptional:true, converter: ParametersConverter.CellConverter);
            AddParameter("from", "f", "The map to modify", isOptional:true, converter: ParametersConverter.MapConverter);
        }


        public override void Execute(TriggerBase trigger)
        {
            var transition = trigger.Get<MapNeighbour>("t");
            var map = trigger.Get<Map>("map");
            var cell = trigger.IsArgumentDefined("cell") ? map.GetCell(trigger.Get<short>("cell")) : null;

            Map from;
            if (trigger.IsArgumentDefined("from"))
                from = trigger.Get<Map>("from");
            else
            {
                if (!(trigger is GameTrigger))
                {
                    trigger.ReplyError("From not defined");
                    return;
                }

                from = (trigger as GameTrigger).Character.Map;
            }

            switch (transition)
            {
                case MapNeighbour.Top:
                    from.TopNeighbour = map;
                    from.Record.TopNeighbourCellId = cell?.Id;
                    break;
                case MapNeighbour.Bottom:
                    from.BottomNeighbour = map;
                    from.Record.BottomNeighbourCellId = cell?.Id;
                    break;
                case MapNeighbour.Right:
                    from.RightNeighbour = map;
                    from.Record.RightNeighbourCellId = cell?.Id;
                    break;
                case MapNeighbour.Left:
                    from.LeftNeighbour = map;
                    from.Record.LeftNeighbourCellId = cell?.Id;
                    break;
                default:
                    trigger.ReplyError("{0} not a valid transition", transition);
                    return;
            }

            WorldServer.Instance.IOTaskPool.ExecuteInContext(() =>
            {
                WorldServer.Instance.DBAccessor.Database.Update(from.Record);
                trigger.ReplyBold("{0} -> {1} = {2}", from.Id, transition, map.Id);
            });
        }
    }

    public class TransitionResetCommand : SubCommand
    {
        public TransitionResetCommand()
        {
            Aliases = new[] {"reset"};
            Description = "Reset the current map transition";
            RequiredRole = RoleEnum.Administrator;
            ParentCommandType = typeof (TransitionsCommands);
            AddParameter("transition", "t", "Top/Right/Bottom/Left",
                converter: ParametersConverter.GetEnumConverter<MapNeighbour>());
            AddParameter("from", "f", "The map to modify", isOptional:true, converter: ParametersConverter.MapConverter);
        }

        public override void Execute(TriggerBase trigger)
        {
            var transition = trigger.Get<MapNeighbour>("t");

            Map from;
            if (trigger.IsArgumentDefined("from"))
                from = trigger.Get<Map>("from");
            else
            {
                if (!(trigger is GameTrigger))
                {
                    trigger.ReplyError("From not defined");
                    return;
                }

                from = (trigger as GameTrigger).Character.Map;
            }

            switch (transition)
            {
                case MapNeighbour.Top:
                    from.TopNeighbour = null;
                    from.Record.TopNeighbourCellId = null;
                    break;
                case MapNeighbour.Bottom:
                    from.BottomNeighbour = null;
                    from.Record.BottomNeighbourCellId = null;
                    break;
                case MapNeighbour.Right:
                    from.RightNeighbour = null;
                    from.Record.RightNeighbourCellId = null;
                    break;
                case MapNeighbour.Left:
                    from.LeftNeighbour = null;
                    from.Record.LeftNeighbourCellId = null;
                    break;
                default:
                    trigger.ReplyError("{0} not a valid transition", transition);
                    return;
            }

            WorldServer.Instance.IOTaskPool.ExecuteInContext(() =>
            {
                WorldServer.Instance.DBAccessor.Database.Update(from.Record);
                trigger.ReplyBold("{0} -> {1} = RESET", from.Id, transition);
            });
        }
    }

    public class TransitionAddTriggerCommand : AddRemoveSubCommand
    {
        public TransitionAddTriggerCommand()
        {
            Aliases = new[] {"trigger"};
            Description = "Add a trigger to the current map";
            RequiredRole = RoleEnum.Administrator;
            ParentCommandType = typeof (TransitionsCommands);
            AddParameter("cellidsrc", "cellsrc", "Cell source", isOptional: true, converter: ParametersConverter.CellConverter);
            AddParameter("map", "map", "Map destination", converter: ParametersConverter.MapConverter);
            AddParameter<short>("celliddst", "celldst", "Cell destination");
        }

        public override void ExecuteAdd(TriggerBase trigger)
        {
            var character = trigger is GameTrigger ? (trigger as GameTrigger).Character : null;

            if (character == null)
                return;

            var map = trigger.Get<Map>("map");

            if (map == null)
            {
                trigger.ReplyError("Map '{0}' doesn't exist", trigger.Get<int>("mapid"));
            }
            else
            {
                var cellIdSrc = trigger.IsArgumentDefined("cellidsrc") ? character.Map.Cells[trigger.Get<short>("cellidsrc")] : character.Cell;
                var cellIdDst = map.Cells[trigger.Get<short>("celliddst")];

                var record = new CellTriggerRecord
                {
                    CellId = cellIdSrc.Id,
                    MapId = character.Map.Id,
                    Type = "Teleport",
                    TriggerType = CellTriggerType.END_MOVE_ON,
                    Parameter0 = cellIdDst.Id.ToString(),
                    Parameter1 = map.Id.ToString()
                };

                WorldServer.Instance.IOTaskPool.ExecuteInContext(() =>
                {
                    CellTriggerManager.Instance.AddCellTrigger(record);
                    trigger.ReplyBold("Add CellTrigger from map {0}({1}) to {2}({3})", record.MapId, record.CellId, record.Parameter1, record.Parameter0);
                });
            }
        }

        public override void ExecuteRemove(TriggerBase trigger)
        {
            var character = trigger is GameTrigger ? (trigger as GameTrigger).Character : null;

            if (character == null)
                return;

            var map = trigger.Get<Map>("map");

            if (map == null)
            {
                trigger.ReplyError("Map '{0}' doesn't exist", trigger.Get<int>("mapid"));
            }
            else
            {
                var cellIdSrc = trigger.IsArgumentDefined("cellidsrc") ? character.Map.Cells[trigger.Get<short>("cellidsrc")] : character.Cell;

                WorldServer.Instance.IOTaskPool.ExecuteInContext(() =>
                {
                    CellTriggerManager.Instance.DeleteCellTrigger(character.Map.Id, cellIdSrc.Id);
                    trigger.ReplyBold("Delete CellTrigger from map {0}({1})", character.Map.Id, cellIdSrc.Id);
                });
            }
        }
    }
}