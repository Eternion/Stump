using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.Server.WorldServer.Database.I18n;
using Stump.Server.WorldServer.Game.Maps;

namespace Stump.Server.WorldServer.Database.World
{
    [Serializable]
    [ActiveRecord("subareas")]
    [D2OClass("SubArea", "com.ankamagames.dofus.datacenter.world")]
    public sealed class SubAreaRecord : WorldBaseRecord<SubAreaRecord>
    {
        private string m_name;

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


        public string Name
        {
            get { return m_name ?? (m_name = TextManager.Instance.GetText(NameId)); }
        }

        [D2OField("areaId")]
        [Property("AreaId")]
        public int AreaId
        {
            get;
            set;
        }

        [D2OField("ambientSounds")]
        [Property("AmbientSounds", ColumnType = "Serializable")]
        public List<AmbientSound> AmbientSounds
        {
            get;
            set;
        }

        [D2OField("mapIds")]
        [Property("MapIds", ColumnType = "Serializable")]
        public List<uint> MapIds
        {
            get;
            set;
        }

        [D2OField("bounds")]
        [Property("Bounds", ColumnType = "Serializable")]
        public Rectangle Bounds
        {
            get;
            set;
        }

        [D2OField("shape")]
        [Property("Shape", ColumnType = "Serializable")]
        public List<int> Shape
        {
            get;
            set;
        }

        [D2OField("customWorldMap")]
        [Property("CustomWorldMap", ColumnType = "Serializable")]
        public List<uint> CustomWorldMap
        {
            get;
            set;
        }

        [D2OField("packId")]
        [Property("PackId")]
        public int PackId
        {
            get;
            set;
        }

        [Property(NotNull = true, Default = "2")]
        public Difficulty Difficulty
        {
            get;
            set;
        }

        [Property(Default = "3")]
        public int SpawnsLimit
        {
            get;
            set;
        }

        [Property(NotNull = false)]
        public uint? CustomSpawnInterval
        {
            get;
            set;
        }
    }
}