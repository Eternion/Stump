

// Generated on 12/20/2015 18:01:09
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Appearance", "com.ankamagames.dofus.datacenter.appearance")]
    [Serializable]
    public class Appearance : IDataObject, IIndexedData
    {
        public const String MODULE = "Appearances";
        public uint id;
        public uint type;
        public String data;
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public uint Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public uint Type
        {
            get { return this.type; }
            set { this.type = value; }
        }
        [D2OIgnore]
        public String Data
        {
            get { return this.data; }
            set { this.data = value; }
        }
    }
}