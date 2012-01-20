using System;
using Castle.ActiveRecord;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Maps.Cells.Triggers;

namespace Stump.Server.WorldServer.Database.World.Triggers
{
    [ActiveRecord("maps_cells_triggers", DiscriminatorColumn = "RecognizerType", DiscriminatorType = "String", DiscriminatorValue = "Base")]
    public abstract class CellTriggerRecord : WorldBaseRecord<CellTriggerRecord>
    {
        private short m_cellId;
        private int m_mapId;
        private bool m_mustRefreshPosition;
        private ObjectPosition m_position;

        [PrimaryKey(PrimaryKeyType.Native)]
        public int Id
        {
            get;
            set;
        }

        [Property]
        public short CellId
        {
            get { return m_cellId; }
            set
            {
                m_cellId = value;
                m_mustRefreshPosition = true;
            }
        }


        [Property]
        public int MapId
        {
            get { return m_mapId; }
            set
            {
                m_mapId = value;
                m_mustRefreshPosition = true;
            }
        }

        [Property]
        public CellTriggerType TriggerType
        {
            get;
            set;
        }

        [Property("`Condition`")]
        public string Condition
        {
            get;
            set;
        }

        private void RefreshPosition()
        {
            Map map = Game.World.Instance.GetMap(MapId);

            if (map == null)
                throw new Exception(string.Format("Cannot load CellTrigger id={0}, map {1} isn't found", Id, MapId));

            Cell cell = map.Cells[CellId];

            m_position = new ObjectPosition(map, cell, DirectionsEnum.DIRECTION_EAST);
        }

        public ObjectPosition GetPosition()
        {
            if (m_position == null || m_mustRefreshPosition)
                RefreshPosition();

            m_mustRefreshPosition = false;

            return m_position;
        }


        public abstract CellTrigger GenerateTrigger();
    }
}