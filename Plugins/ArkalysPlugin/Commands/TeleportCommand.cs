using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace ArkalysPlugin.Commands
{
    public class TPCommands : SubCommandContainer
    {
        public TPCommands()
        {
            Aliases = new[] { "tp" };
            RequiredRole = RoleEnum.Player;
            Description = "Teleport Commands";
        }
    }

    public class BetaTPCommand : InGameSubCommand
    {
        public BetaTPCommand()
        {
            Aliases = new[] { "beta" };
            RequiredRole = RoleEnum.Player;
            Description = "Téléporte à la zone bêta";
            ParentCommandType = typeof(TPCommands);
        }

        public override void Execute(GameTrigger trigger)
        {
            var map = World.Instance.GetMap(154272513);

            if (map == null)
            {
                trigger.ReplyError("Map {0} not found", 154272513);
                return;
            }

            var cell = map.Cells[456];

            trigger.Character.Teleport(new ObjectPosition(map, cell, DirectionsEnum.DIRECTION_SOUTH));
            trigger.Reply("Téléporté à la zone bêta");
        }
    }
}
