
// Generated on 03/02/2013 21:17:43
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("AlmanaxCalendars")]
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

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        public uint DescId
        {
            get { return descId; }
            set { descId = value; }
        }

        public int NpcId
        {
            get { return npcId; }
            set { npcId = value; }
        }

    }
}