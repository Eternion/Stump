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
            RequiredRole = RoleEnum.Administrator;
            Description = "Teleport Commands";
        }
    }

    public class NpcTPCommand : InGameSubCommand
    {
        [Variable(true)]
        public static int NpcMap;

        [Variable(true)]
        public static short NpcCell;

        [Variable(true)]
        public static byte NpcDirection;

        public NpcTPCommand()
        {
            Aliases = new[] { "pnj" };
            RequiredRole = RoleEnum.Player;
            Description = "Téléporte à la zone d'achat d'équipements";
            ParentCommandType = typeof (TPCommands);
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

    public class ShopTPCommand : InGameSubCommand
    {
        [Variable(true)]
        public static int ShopMap;

        [Variable(true)]
        public static short ShopCell;

        [Variable(true)]
        public static byte ShopDirection;

        public ShopTPCommand()
        {
            Aliases = new[] { "shop", "boutique" };
            RequiredRole = RoleEnum.Player;
            Description = "Téléporte à l'espace boutique";
            ParentCommandType = typeof (TPCommands);
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

    public class BankTPCommand : InGameSubCommand
    {
        [Variable(true)]
        public static int BankMap;

        [Variable(true)]
        public static short BankCell;

        [Variable(true)]
        public static byte BankDirection;

        public BankTPCommand()
        {
            Aliases = new[] { "bank", "banque" };
            RequiredRole = RoleEnum.Player;
            Description = "Téléporte à la banque";
            ParentCommandType = typeof (TPCommands);
        }

        public override void Execute(GameTrigger trigger)
        {
            var map = World.Instance.GetMap(BankMap);

            if (map == null)
            {
                trigger.ReplyError("Map {0} not found", BankMap);
                return;
            }

            var cell = map.Cells[BankCell];

            trigger.Character.Teleport(new ObjectPosition(map, cell, (DirectionsEnum)BankDirection));
            trigger.Reply("Téléporté à la banque");
        }
    }
}
