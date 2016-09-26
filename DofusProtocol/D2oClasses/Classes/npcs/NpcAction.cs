

// Generated on 09/26/2016 01:50:31
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("NpcAction", "com.ankamagames.dofus.datacenter.npcs")]
    [Serializable]
    public class NpcAction : IDataObject, IIndexedData
    {
        public const String MODULE = "NpcActions";
        public int id;
        public int realId;
        [I18NField]
        public uint nameId;
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
        public int RealId
        {
            get { return this.realId; }
            set { this.realId = value; }
        }
        [D2OIgnore]
        public uint NameId
        {
            get { return this.nameId; }
            set { this.nameId = value; }
        }
    }
}