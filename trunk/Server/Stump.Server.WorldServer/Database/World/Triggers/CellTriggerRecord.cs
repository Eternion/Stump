using System;
using System.Data.Entity.ModelConfiguration;
using Castle.ActiveRecord;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Maps.Cells.Triggers;

namespace Stump.Server.WorldServer.Database.Triggers
{
    public class CellTriggerRecordConfiguration : EntityTypeConfiguration<CellTriggerRecord>
    {
        public CellTriggerRecordConfiguration()
        {
            ToTable("maps_triggers");
            Map(x => x.Requires("Discriminator").HasValue("Base"));
        }
    }

    public abstract class CellTriggerRecord : WorldBaseRecord<CellTriggerRecord>
    {
        private short m_cellId;
        private int m_mapId;
        private bool m_mustRefreshPosition;
        private ObjectPosition m_position;

        public int Id
        {
            get;
            set;
        }

        public short CellId
        {
            get { return m_cellId; }
            set
            {
                m_cellId = value;
                m_mustRefreshPosition = true;
            }
        }

        public int MapId
        {
            get { return m_mapId; }
            set
            {
                m_mapId = value;
                m_mustRefreshPosition = true;
            }
        }

        public int TriggerTypeInt
        {
            get;
            set;
        }

        public CellTriggerType TriggerType
        {
            get { return (CellTriggerType) TriggerTypeInt; }
            set { TriggerTypeInt = (int) value; }
        }

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