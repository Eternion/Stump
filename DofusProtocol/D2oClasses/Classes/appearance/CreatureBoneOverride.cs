

// Generated on 02/11/2015 10:21:30
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("CreatureBoneOverride", "com.ankamagames.dofus.datacenter.appearance")]
    [Serializable]
    public class CreatureBoneOverride : IDataObject, IIndexedData
    {
        public const String MODULE = "CreatureBonesOverrides";
        public int boneId;
        public int creatureBoneId;
        int IIndexedData.Id
        {
            get { return (int)boneId; }
        }
        [D2OIgnore]
        public int BoneId
        {
            get { return this.boneId; }
            set { this.boneId = value; }
        }
        [D2OIgnore]
        public int CreatureBoneId
        {
            get { return this.creatureBoneId; }
            set { this.creatureBoneId = value; }
        }
    }
}