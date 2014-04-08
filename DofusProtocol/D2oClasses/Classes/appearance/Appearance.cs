

// Generated on 10/28/2013 14:03:17
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
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
        public uint Type
        {
            get { return type; }
            set { type = value; }
        }
        [D2OIgnore]
        public String Data
        {
            get { return data; }
            set { data = value; }
        }
    }
}