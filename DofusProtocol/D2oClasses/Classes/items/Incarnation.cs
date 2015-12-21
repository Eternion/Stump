

// Generated on 12/20/2015 18:01:12
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Incarnation", "com.ankamagames.dofus.datacenter.items")]
    [Serializable]
    public class Incarnation : IDataObject, IIndexedData
    {
        public const String MODULE = "Incarnation";
        public uint id;
        public String lookMale;
        public String lookFemale;
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
        public String LookMale
        {
            get { return this.lookMale; }
            set { this.lookMale = value; }
        }
        [D2OIgnore]
        public String LookFemale
        {
            get { return this.lookFemale; }
            set { this.lookFemale = value; }
        }
    }
}