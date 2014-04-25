

// Generated on 10/28/2013 14:03:20
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
        private const String MODULE = "NpcActions";
        public int id;
        [I18NField]
        public uint nameId;
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }
    }
}