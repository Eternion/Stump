using System;
using System.Collections;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Server.WorldServer.Database.World
{
    [Serializable]
    [ActiveRecord("superAreas")]
    [D2OClass("SuperArea", "com.ankamagames.dofus.datacenter.world")]
    public sealed class SuperAreaRecord : WorldBaseRecord<SuperAreaRecord>
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
                if (value != null)
                    WorldmapId = (uint) value.Id;
                m_worldMap = value;
            }
        }

        protected override bool BeforeSave(IDictionary state)
        {
            // that's a hack to update SubAreaId field without setting SubArea property.
            if (WorldMap == null && WorldmapId > 0)
                WorldMap = new WorldMapRecord
                {
                    Id = (int) WorldmapId
                };


            return base.BeforeSave(state);
        }
    }
}