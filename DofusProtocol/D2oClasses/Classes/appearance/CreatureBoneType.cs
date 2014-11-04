

// Generated on 10/26/2014 23:27:34
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("CreatureBoneType", "com.ankamagames.dofus.datacenter.appearance")]
    [Serializable]
    public class CreatureBoneType : IDataObject, IIndexedData
    {
        public const String MODULE = "CreatureBonesTypes";
        public int id;
        public int creatureBoneId;
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
        public int CreatureBoneId
        {
            get { return this.creatureBoneId; }
            set { this.creatureBoneId = value; }
        }
    }
}