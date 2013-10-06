

// Generated on 10/06/2013 17:58:52
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("AlmanaxCalendar", "com.ankamagames.dofus.datacenter.almanax")]
    [Serializable]
    public class AlmanaxCalendar : IDataObject, IIndexedData
    {
        private const String MODULE = "AlmanaxCalendars";
        public int id;
        public uint nameId;
        public uint descId;
        public int npcId;
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
        [D2OIgnore]
        public uint DescId
        {
            get { return descId; }
            set { descId = value; }
        }
        [D2OIgnore]
        public int NpcId
        {
            get { return npcId; }
            set { npcId = value; }
        }
    }
}