using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Dialogs;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Handlers;
using Stump.Server.WorldServer.Handlers.Dialogs;
using System.Collections.Generic;
using System.Linq;

namespace GameplayPlugin.Commands
{
    public class TPCommands : InGameCommand
    {
        [Variable(definableByConfig: true, DefinableRunning = true)]
        public static List<TeleportMap> Destinations = new List<TeleportMap>
        {
                new TeleportMap(162272258, 224, 2000),
                new TeleportMap(162267138, 51, 2001),
                new TeleportMap(88212759, 287, 2002),
                new TeleportMap(84674563, 315, 2003)
        };

        public TPCommands()
        {
            Aliases = new[] { "tp" };
            RequiredRole = RoleEnum.Player;
            Description = "Teleport Commands";
        }

        public override void Execute(GameTrigger trigger)
        {
            var dialog = new TeleportDialog(trigger.Character, Destinations);
            dialog.Open();
        }
    }

    public class TeleportMap
    {
        public int MapId;
        public int CellId;
        public short SubAreaId;

        public TeleportMap() { }

        public TeleportMap(int mapId, int cellId, short subAreaId)
        {
            MapId = mapId;
            CellId = cellId;
            SubAreaId = subAreaId;
        }
    }

    public class TeleportHandler : WorldHandlerContainer
    {
        [Initialization(InitializationPass.Last, Silent = true)]
        public static void Initialize()
        {
            WorldServer.Instance.HandlerManager.Register(typeof(TeleportHandler));
        }

        [WorldHandler(TeleportRequestMessage.Id)]
        public static void HandleTeleportRequestMessage(WorldClient client, TeleportRequestMessage message)
        {
            var dialog = client.Character.Dialog as TeleportDialog;

            if (dialog == null)
                return;

            var map = World.Instance.GetMap(message.mapId);

            if (map == null)
                return;

            dialog.Teleport(map);
        }
    }

    public class TeleportDialog : IDialog
    {
        private readonly List<TeleportMap> m_destinations;

        public TeleportDialog(Character character, IEnumerable<TeleportMap> destinations)
        {
            Character = character;
            m_destinations = destinations.ToList();
        }

        public DialogTypeEnum DialogType => DialogTypeEnum.DIALOG_TELEPORTER;

        public Character Character
        {
            get;
        }

        public void Open()
        {
            Character.SetDialog(this);
            SendZaapListMessage(Character.Client);
        }

        public void Close()
        {
            Character.CloseDialog(this);
            DialogHandler.SendLeaveDialogMessage(Character.Client, DialogType);
        }

        public void Teleport(Map map)
        {
            var destination = m_destinations.FirstOrDefault(x => x.MapId == map.Id);
            if (destination == null)
                return;

            var cell = map.GetCell(destination.CellId);
            if (cell == null)
                return;

            Character.Teleport(map, cell);

            Close();
        }

        public void SendZaapListMessage(IPacketReceiver client)
        {
            client.Send(new ZaapListMessage((sbyte)TeleporterTypeEnum.TELEPORTER_ZAAP,
                m_destinations.Select(entry => entry.MapId),
                m_destinations.Select(entry => entry.SubAreaId),
                m_destinations.Select(x => (short)0),
                m_destinations.Select(x => (sbyte)TeleporterTypeEnum.TELEPORTER_ZAAP),
                Character.Map.Id));
        }
    }
}