using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Handlers.Dialogs;

namespace Stump.Server.WorldServer.Game.Dialogs.Interactives
{
    public class ZaapiDialog : IDialog
    {
        private readonly List<Map> m_destinations = new List<Map>();

        public ZaapiDialog(Character character, InteractiveObject zaapi)
        {
            Character = character;
            Zaapi = zaapi;

            foreach (var map in from map in character.Area.Maps from interactive in map.GetInteractiveObjects()
                                    .Where(interactive => interactive.Template != null && interactive.Template.Type == InteractiveTypeEnum.TYPE_ZAAPI) select map)
            {
                AddDestination(map);
            }
        }

        public DialogTypeEnum DialogType
        {
            get { return DialogTypeEnum.DIALOG_TELEPORTER; }
        }

        public Character Character
        {
            get;
            private set;
        }

        public InteractiveObject Zaapi
        {
            get;
            private set;
        }

        public void AddDestination(Map map)
        {
            m_destinations.Add(map);
        }

        public void Open()
        {
            Character.SetDialog(this);
            SendZaapiListMessage(Character.Client);
        }

        public void Close()
        {
            Character.CloseDialog(this);
            DialogHandler.SendLeaveDialogMessage(Character.Client, DialogType);
        }

        public void Teleport(Map map)
        {
            if (!m_destinations.Contains(map))
                return;

            var cell = map.GetCell(280);

            if (map.Zaapi != null)
            {
                cell =
                    map.GetCell(
                        map.Zaapi.Position.Point.GetCellInDirection(DirectionsEnum.DIRECTION_SOUTH_WEST, 1).CellId);

                if (!cell.Walkable)
                {
                    var adjacents = map.Zaapi.Position.Point.GetAdjacentCells(entry => map.GetCell(entry).Walkable).ToArray();

                    if (adjacents.Length == 0)
                        throw new Exception(string.Format("Cannot find a free adjacent cell near the zaapi (id:{0}) on map {1}", map.Zaapi.Id, map.Id));

                    cell = map.GetCell(adjacents[0].CellId);
                }
            }

            var cost = GetCostTo(map);

            if (Character.Kamas < cost)
                return;

            Character.Inventory.SubKamas(cost);
            Character.Teleport(map, cell);

            Close();
        }

        public void SendZaapiListMessage(IPacketReceiver client)
        {
            client.Send(new TeleportDestinationsListMessage(
                (sbyte)TeleporterTypeEnum.TELEPORTER_SUBWAY,
                m_destinations.Select(entry => entry.Id),
                m_destinations.Select(entry => (short)entry.SubArea.Id),
                m_destinations.Select(GetCostTo)));
        }

        public short GetCostTo(Map map)
        {
            var pos = map.Position;
            var pos2 = Zaapi.Map.Position;

            return (short)Math.Floor(Math.Sqrt((pos2.X - pos.X) * (pos2.X - pos.X) + (pos2.Y - pos.Y) * (pos2.Y - pos.Y)) * 10);
        }
    }
}
