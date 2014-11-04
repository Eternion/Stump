

// Generated on 10/26/2014 23:27:54
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("MapScrollAction", "com.ankamagames.dofus.datacenter.world")]
    [Serializable]
    public class MapScrollAction : IDataObject, IIndexedData
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
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public Boolean RightExists
        {
            get { return this.rightExists; }
            set { this.rightExists = value; }
        }
        [D2OIgnore]
        public Boolean BottomExists
        {
            get { return this.bottomExists; }
            set { this.bottomExists = value; }
        }
        [D2OIgnore]
        public Boolean LeftExists
        {
            get { return this.leftExists; }
            set { this.leftExists = value; }
        }
        [D2OIgnore]
        public Boolean TopExists
        {
            get { return this.topExists; }
            set { this.topExists = value; }
        }
        [D2OIgnore]
        public int RightMapId
        {
            get { return this.rightMapId; }
            set { this.rightMapId = value; }
        }
        [D2OIgnore]
        public int BottomMapId
        {
            get { return this.bottomMapId; }
            set { this.bottomMapId = value; }
        }
        [D2OIgnore]
        public int LeftMapId
        {
            get { return this.leftMapId; }
            set { this.leftMapId = value; }
        }
        [D2OIgnore]
        public int TopMapId
        {
            get { return this.topMapId; }
            set { this.topMapId = value; }
        }
    }
}