using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Game.Maps;

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
                    from.TopNeighbourCell = cell;
                    break;
                case MapNeighbour.Bottom:
                    from.BottomNeighbour = map;
                    from.BottomNeighbourCell = cell;
                    break;
                case MapNeighbour.Right:
                    from.RightNeighbour = map;
                    from.RightNeighbourCell = cell;
                    break;
                case MapNeighbour.Left:
                    from.LeftNeighbour = map;
                    from.LeftNeighbourCell = cell;
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
                    from.TopNeighbourCell = null;
                    break;
                case MapNeighbour.Bottom:
                    from.BottomNeighbour = null;
                    from.BottomNeighbourCell = null;
                    break;
                case MapNeighbour.Right:
                    from.RightNeighbour = null;
                    from.RightNeighbourCell = null;
                    break;
                case MapNeighbour.Left:
                    from.LeftNeighbour = null;
                    from.LeftNeighbourCell = null;
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
}