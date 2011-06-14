using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.World
{
    [Serializable]
    [ActiveRecord("super_area")]
    [AttributeAssociatedFile("SuperAreas")]
    [D2OClass("SuperArea", "com.ankamagames.dofus.datacenter.world")]
    public sealed class SuperAreaRecord : DataBaseRecord<SuperAreaRecord>
    {

        [D2OField("id")]
        [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
        public int Id
        {
            get;
            set;
        }

        [D2OField("nameId")]
        [Property("NameId")]
        public uint NameId
        {
            get;
            set;
        }

        [D2OField("worldmapId")]
        public uint WorldmapId
        {
            get;
            set;
        }

        private WorldMapRecord m_worldMap;

        [BelongsTo("WorldMapId")]
        public WorldMapRecord WorldMap
        {
            get
            {
                return m_worldMap;
            }
            set
            {
                m_worldMap = value;
                WorldmapId = (uint)value.Id;
            }
        }
    }
}