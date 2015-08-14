 


// Generated on 08/13/2015 17:50:48
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
    [TableName("MapScrollActions")]
    [D2OClass("MapScrollAction", "com.ankamagames.dofus.datacenter.world")]
    public class MapScrollActionRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "MapScrollActions";
        public int id;
        public Boolean rightExists;
        public Boolean bottomExists;
        public Boolean leftExists;
        public Boolean topExists;
        public int rightMapId;
        public int bottomMapId;
        public int leftMapId;
        public int topMapId;

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
        public Boolean RightExists
        {
            get { return rightExists; }
            set { rightExists = value; }
        }

        [D2OIgnore]
        public Boolean BottomExists
        {
            get { return bottomExists; }
            set { bottomExists = value; }
        }

        [D2OIgnore]
        public Boolean LeftExists
        {
            get { return leftExists; }
            set { leftExists = value; }
        }

        [D2OIgnore]
        public Boolean TopExists
        {
            get { return topExists; }
            set { topExists = value; }
        }

        [D2OIgnore]
        public int RightMapId
        {
            get { return rightMapId; }
            set { rightMapId = value; }
        }

        [D2OIgnore]
        public int BottomMapId
        {
            get { return bottomMapId; }
            set { bottomMapId = value; }
        }

        [D2OIgnore]
        public int LeftMapId
        {
            get { return leftMapId; }
            set { leftMapId = value; }
        }

        [D2OIgnore]
        public int TopMapId
        {
            get { return topMapId; }
            set { topMapId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (MapScrollAction)obj;
            
            Id = castedObj.id;
            RightExists = castedObj.rightExists;
            BottomExists = castedObj.bottomExists;
            LeftExists = castedObj.leftExists;
            TopExists = castedObj.topExists;
            RightMapId = castedObj.rightMapId;
            BottomMapId = castedObj.bottomMapId;
            LeftMapId = castedObj.leftMapId;
            TopMapId = castedObj.topMapId;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (MapScrollAction)parent : new MapScrollAction();
            obj.id = Id;
            obj.rightExists = RightExists;
            obj.bottomExists = BottomExists;
            obj.leftExists = LeftExists;
            obj.topExists = TopExists;
            obj.rightMapId = RightMapId;
            obj.bottomMapId = BottomMapId;
            obj.leftMapId = LeftMapId;
            obj.topMapId = TopMapId;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}