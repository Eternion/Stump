 


// Generated on 10/06/2013 01:11:00
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [D2OClass("Areas")]
    public class AreaRecord : ID2ORecord
    {
        private const String MODULE = "Areas";
        public int id;
        public uint nameId;
        public int superAreaId;
        public Boolean containHouses;
        public Boolean containPaddocks;
        public Rectangle bounds;

        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        public int SuperAreaId
        {
            get { return superAreaId; }
            set { superAreaId = value; }
        }

        public Boolean ContainHouses
        {
            get { return containHouses; }
            set { containHouses = value; }
        }

        public Boolean ContainPaddocks
        {
            get { return containPaddocks; }
            set { containPaddocks = value; }
        }

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
        public byte[] BoundsBin
        {
            get { return m_boundsBin; }
            set
            {
                m_boundsBin = value;
                bounds = value == null ? null : value.ToObject<Rectangle>();
            }
        }

        public void AssignFields(object obj)
        {
            var castedObj = (Area)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            SuperAreaId = castedObj.superAreaId;
            ContainHouses = castedObj.containHouses;
            ContainPaddocks = castedObj.containPaddocks;
            Bounds = castedObj.bounds;
        }
        
        public object CreateObject()
        {
            var obj = new Area();
            
            obj.id = Id;
            obj.nameId = NameId;
            obj.superAreaId = SuperAreaId;
            obj.containHouses = ContainHouses;
            obj.containPaddocks = ContainPaddocks;
            obj.bounds = Bounds;
            return obj;
        
        }
    }
}