

// Generated on 12/12/2013 16:57:38
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
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
        public String LookMale
        {
            get { return lookMale; }
            set { lookMale = value; }
        }
        [D2OIgnore]
        public String LookFemale
        {
            get { return lookFemale; }
            set { lookFemale = value; }
        }
    }
}