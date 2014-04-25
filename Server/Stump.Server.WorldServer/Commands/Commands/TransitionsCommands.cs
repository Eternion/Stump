using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
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
            RequiredRole = RoleEnum.GameMaster;
        }
         
    }

    public class TransitionSetCommand : SubCommand
    {
        public TransitionSetCommand()
        {
            Aliases = new[] {"set"};
            Description = "Set the current map transition";
            RequiredRole  = RoleEnum.GameMaster;
            ParentCommandType = typeof (TransitionsCommands);
            AddParameter("transition", "t", "Top/Right/Bottom/Left",
                converter: ParametersConverter.GetEnumConverter<MapNeighbour>());
            AddParameter("map", "m", "The destination", converter: ParametersConverter.MapConverter);
            AddParameter("from", "f", "The map to modify", isOptional:true, converter: ParametersConverter.MapConverter);
        }


        public override void Execute(TriggerBase trigger)
        {
            var transition = trigger.Get<MapNeighbour>("t");
            var map = trigger.Get<Map>("map");

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
                    break;
                case MapNeighbour.Bottom:
                    from.BottomNeighbour = map;
                    break;
                case MapNeighbour.Right:
                    from.RightNeighbour = map;
                    break;
                case MapNeighbour.Left:
                    from.LeftNeighbour = map;
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
            RequiredRole  = RoleEnum.GameMaster;
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
                    break;
                case MapNeighbour.Bottom:
                    from.BottomNeighbour = null;
                    break;
                case MapNeighbour.Right:
                    from.RightNeighbour = null;
                    break;
                case MapNeighbour.Left:
                    from.LeftNeighbour = null;
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