using System.Drawing;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Core.Extensions;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class FightPlacementCommands : SubCommandContainer
    {
        public FightPlacementCommands()
        {
            Aliases = new[] {"placement"};
            Description = "Manage fight placement";
            RequiredRole = RoleEnum.GameMaster;
        }
    }

    public class FightPlacementShowCommand : InGameSubCommand
    {
        public FightPlacementShowCommand()
        {
            Aliases = new[] {"show"};
            ParentCommandType = typeof (FightPlacementCommands);
            Description = "Display current map placements";
            RequiredRole = RoleEnum.GameMaster;
        }

        public override void Execute(GameTrigger trigger)
        {
            trigger.Character.Client.Send(new DebugClearHighlightCellsMessage());

            var blue = trigger.Character.Map.GetBlueFightPlacement();
            var red = trigger.Character.Map.GetRedFightPlacement();
            if (blue == null || blue.Length == 0)
            {
                trigger.ReplyError("Blue placements not defined");
            }
            else
                trigger.Character.Client.Send(new DebugHighlightCellsMessage(Color.Blue.ToArgb(), blue.Select(x => x.Id)));

            if (red == null || red.Length == 0)
            {
                trigger.ReplyError("Red placements not defined");
            }
            else
                trigger.Character.Client.Send(new DebugHighlightCellsMessage(Color.Red.ToArgb(), red.Select(x => x.Id)));
        }
    }

    public class FightPlacementClearCommand : InGameSubCommand
    {
        public FightPlacementClearCommand()
        {
            Aliases = new[] { "clear" };
            ParentCommandType = typeof(FightPlacementCommands);
            Description = "Clear current map placements";
            RequiredRole = RoleEnum.GameMaster;
            AddParameter<string>("color", "c", "Blue/Red");
        }

        public override void Execute(GameTrigger trigger)
        {
            var colorStr = trigger.Get<string>("color").ToLower();

            if (colorStr != "red" && colorStr != "blue")
            {
                trigger.ReplyError("Define a correct color (blue/red)");
                return;
            }

            var blue = colorStr == "blue";

            WorldServer.Instance.IOTaskPool.ExecuteInContext(() =>
            {
                if (blue)
                    trigger.Character.Map.Record.BlueFightCells = new short[0];
                else
                    trigger.Character.Map.Record.RedFightCells = new short[0];

                trigger.Character.Map.UpdateFightPlacements();

                WorldServer.Instance.DBAccessor.Database.Update(trigger.Character.Map.Record);
            });

            trigger.Character.Client.Send(new DebugClearHighlightCellsMessage());
        }
    }

    public class FightPlacementSetCommand : InGameSubCommand
    {
        public FightPlacementSetCommand()
        {
            Aliases = new[] {"set"};            
            ParentCommandType = typeof (FightPlacementCommands);
            Description = "Set current map placements";
            RequiredRole = RoleEnum.GameMaster;
            AddParameter<string>("color", "c", "Blue/Red");
            AddParameter<string>("cells", "cells", "cell#1,cell#2,cell#3...");
        }

        public override void Execute(GameTrigger trigger)
        {
            var colorStr = trigger.Get<string>("color").ToLower();

            if (colorStr != "red" && colorStr != "blue")
            {
                trigger.ReplyError("Define a correct color (blue/red)");
                return;
            }

            var blue = colorStr == "blue";

            var cellsStr = trigger.Get<string>("cells").Split(',');

            var cells = cellsStr.Select(x =>
            {
                int id;
                if (!int.TryParse(x, out id) || id < 0 || id > 559)
                    throw new ConverterException(string.Format("{0} is not a valid cell id", x));

                var cell = trigger.Character.Map.GetCell(id);
                if (!cell.Walkable)
                    throw new ConverterException(string.Format("Cell {0} is not walkable", x));

                return cell;
            });

            WorldServer.Instance.IOTaskPool.ExecuteInContext(() =>
            {
                if (blue)
                    trigger.Character.Map.Record.BlueFightCells = cells.Select(x => x.Id).ToArray();
                else
                    trigger.Character.Map.Record.RedFightCells = cells.Select(x => x.Id).ToArray();

                trigger.Character.Map.UpdateFightPlacements();

                trigger.Character.Client.Send(new DebugHighlightCellsMessage(Color.Blue.ToArgb(), trigger.Character.Map.Record.BlueFightCells));
                trigger.Character.Client.Send(new DebugHighlightCellsMessage(Color.Red.ToArgb(), trigger.Character.Map.Record.RedFightCells));

                WorldServer.Instance.DBAccessor.Database.Update(trigger.Character.Map.Record);
            });
        }
    }

    public class FightPlacementAddCommand : InGameSubCommand
    {
        public FightPlacementAddCommand()
        {
            Aliases = new[] { "add" };
            ParentCommandType = typeof(FightPlacementCommands);
            Description = "Add current cell to map placements";
            RequiredRole = RoleEnum.GameMaster;
            AddParameter<string>("color", "c", "Blue/Red");
        }

        public override void Execute(GameTrigger trigger)
        {
            var colorStr = trigger.Get<string>("color").ToLower();

            if (colorStr != "red" && colorStr != "blue")
            {
                trigger.ReplyError("Define a correct color (blue/red)");
                return;
            }

            var blue = colorStr == "blue";

            var cell = trigger.Character.Cell;

            WorldServer.Instance.IOTaskPool.ExecuteInContext(() =>
            {
                if (blue)
                    trigger.Character.Map.Record.BlueFightCells = trigger.Character.Map.Record.BlueFightCells.Add(cell.Id);
                else
                    trigger.Character.Map.Record.RedFightCells = trigger.Character.Map.Record.RedFightCells.Add(cell.Id);

                trigger.Character.Map.UpdateFightPlacements();

                trigger.Character.Client.Send(new DebugHighlightCellsMessage(Color.Blue.ToArgb(), trigger.Character.Map.Record.BlueFightCells));
                trigger.Character.Client.Send(new DebugHighlightCellsMessage(Color.Red.ToArgb(), trigger.Character.Map.Record.RedFightCells));

                WorldServer.Instance.DBAccessor.Database.Update(trigger.Character.Map.Record);
            });

        }
    }
}