using System;
using Castle.ActiveRecord;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Maps.Cells.Triggers;

namespace Stump.Server.WorldServer.Database.World.Triggers
{
    [ActiveRecord(DiscriminatorValue = "Teleport")]
    public class TeleportTriggerRecord : CellTriggerRecord
    {
        private short m_destinationCellId;

        private int m_destinationMapId;
        private ObjectPosition m_destinationPosition;
        private bool m_mustRefreshDestinationPosition;

        [Property]
        public short DestinationCellId
        {
            get { return m_destinationCellId; }
            set
            {
                m_destinationCellId = value;
                m_mustRefreshDestinationPosition = true;
            }
        }

        [Property]
        public int DestinationMapId
        {
            get { return m_destinationMapId; }
            set
            {
                m_destinationMapId = value;
                m_mustRefreshDestinationPosition = true;
            }
        }

        private void RefreshPosition()
        {
            Map map = Game.World.Instance.GetMap(DestinationMapId);

            if (map == null)
                throw new Exception(string.Format("Cannot load CellTeleportTrigger id={0}, map {1} isn't found", Id, MapId));

            Cell cell = map.Cells[DestinationCellId];

            m_destinationPosition = new ObjectPosition(map, cell, DirectionsEnum.DIRECTION_EAST);
        }

        public ObjectPosition GetDestinationPosition()
        {
            if (m_destinationPosition == null || m_mustRefreshDestinationPosition)
                RefreshPosition();

            m_mustRefreshDestinationPosition = false;

            return m_destinationPosition;
        }

        public override CellTrigger GenerateTrigger()
        {
            return new TeleportTrigger(this);
        }
    }
}