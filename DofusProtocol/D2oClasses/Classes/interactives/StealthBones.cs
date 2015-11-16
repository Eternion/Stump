

// Generated on 11/16/2015 14:26:30
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("StealthBones", "com.ankamagames.dofus.datacenter.interactives")]
    [Serializable]
    public class StealthBones : IDataObject, IIndexedData
    {
        public const String MODULE = "StealthBones";
        public uint id;
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
    }
}