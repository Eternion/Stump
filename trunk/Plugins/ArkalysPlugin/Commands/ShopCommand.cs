using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace ArkalysPlugin.Commands
{
    public class ShopCommand : InGameCommand
    {
        [Variable(true)]
        public static int ShopMap;

        [Variable(true)]
        public static short ShopCell;

        [Variable(true)]
        public static byte ShopDirection;

        public ShopCommand()
        {
            Aliases = new[] { "shop", "boutique" };
            RequiredRole = RoleEnum.Player;
            Description = "Téléporte à l'espace boutique";
        }

        public override void Execute(GameTrigger trigger)
        {
            var map = World.Instance.GetMap(ShopMap);

            if (map == null)
            {
                trigger.ReplyError("Map {0} not found", ShopMap);
                return;
            }

            var cell = map.Cells[ShopCell];

            trigger.Character.Teleport(new ObjectPosition(map, cell, (DirectionsEnum)ShopDirection));
            trigger.Reply("Téléporté au shop");
        }
    }
}