using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace ArkalysPlugin.Commands
{
    class NpcCommand : InGameCommand
    {
        [Variable(true)]
        public static int NpcMap;

        [Variable(true)]
        public static short NpcCell;

        [Variable(true)]
        public static byte NpcDirection;

        public NpcCommand()
        {
            Aliases = new[] { "pnj" };
            RequiredRole = RoleEnum.Player;
            Description = "Téléporte à la zone d'achat d'équipements";
        }

        public override void Execute(GameTrigger trigger)
        {
            var map = World.Instance.GetMap(NpcMap);

            if (map == null)
            {
                trigger.ReplyError("Map {0} not found", NpcMap);
                return;
            }

            var cell = map.Cells[NpcCell];

            trigger.Character.Teleport(new ObjectPosition(map, cell, (DirectionsEnum)NpcDirection));
            trigger.Reply("Téléporté à la zone d'achat d'équipements");
        }
    }
}
