
// Generated on 03/25/2013 19:24:36
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("MonsterRaces")]
    [Serializable]
    public class MonsterRace : IDataObject, IIndexedData
    {
        private const String MODULE = "MonsterRaces";
        public int id;
        public int superRaceId;
        public uint nameId;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int SuperRaceId
        {
            get { return superRaceId; }
            set { superRaceId = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

    }
}