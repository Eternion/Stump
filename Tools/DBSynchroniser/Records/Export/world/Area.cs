 


// Generated on 09/01/2015 10:48:50
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("Areas")]
    [D2OClass("Area", "com.ankamagames.dofus.datacenter.world")]
    public class AreaRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "Areas";
        public int id;
        [I18NField]
        public uint nameId;
        public int superAreaId;
        public Boolean containHouses;
        public Boolean containPaddocks;
        public Rectangle bounds;
        public uint worldmapId;
        public Boolean hasWorldMap;

        int ID2ORecord.Id
        {
            get { return (int)id; }
        }


        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        [I18NField]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        [D2OIgnore]
        public int SuperAreaId
        {
            get { return superAreaId; }
            set { superAreaId = value; }
        }

        [D2OIgnore]
        public Boolean ContainHouses
        {
            get { return containHouses; }
            set { containHouses = value; }
        }

        [D2OIgnore]
        public Boolean ContainPaddocks
        {
            get { return containPaddocks; }
            set { containPaddocks = value; }
        }

        [D2OIgnore]
        [Ignore]
        public Rectangle Bounds
        {
            get { return bounds; }
            set
            {
                bounds = value;
                m_boundsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_boundsBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] BoundsBin
        {
            get { return m_boundsBin; }
            set
            {
                m_boundsBin = value;
                bounds = value == null ? null : value.ToObject<Rectangle>();
            }
        }

        [D2OIgnore]
        public uint WorldmapId
        {
            get { return worldmapId; }
            set { worldmapId = value; }
        }

        [D2OIgnore]
        public Boolean HasWorldMap
        {
            get { return hasWorldMap; }
            set { hasWorldMap = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Area)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            SuperAreaId = castedObj.superAreaId;
            ContainHouses = castedObj.containHouses;
            ContainPaddocks = castedObj.containPaddocks;
            Bounds = castedObj.bounds;
            WorldmapId = castedObj.worldmapId;
            HasWorldMap = castedObj.hasWorldMap;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (Area)parent : new Area();
            obj.id = Id;
            obj.nameId = NameId;
            obj.superAreaId = SuperAreaId;
            obj.containHouses = ContainHouses;
            obj.containPaddocks = ContainPaddocks;
            obj.bounds = Bounds;
            obj.worldmapId = WorldmapId;
            obj.hasWorldMap = HasWorldMap;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
            m_boundsBin = bounds == null ? null : bounds.ToBinary();
        
        }
    }
}