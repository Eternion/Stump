

// Generated on 12/12/2013 16:57:41
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("MonsterRace", "com.ankamagames.dofus.datacenter.monsters")]
    [Serializable]
    public class MonsterRace : IDataObject, IIndexedData
    {
        public const String MODULE = "MonsterRaces";
        public int id;
        public int superRaceId;
        [I18NField]
        public uint nameId;
        public List<uint> monsters;
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
        public int SuperRaceId
        {
            get { return superRaceId; }
            set { superRaceId = value; }
        }
        [D2OIgnore]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }
        [D2OIgnore]
        public List<uint> Monsters
        {
            get { return monsters; }
            set { monsters = value; }
        }
    }
}