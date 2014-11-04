

// Generated on 10/26/2014 23:27:53
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("SoundUiHook", "com.ankamagames.dofus.datacenter.sounds")]
    [Serializable]
    public class SoundUiHook : IDataObject, IIndexedData
    {
        public String MODULE = "SoundUiHook";
        public uint id;
        public String name;
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
        public String Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
    }
}