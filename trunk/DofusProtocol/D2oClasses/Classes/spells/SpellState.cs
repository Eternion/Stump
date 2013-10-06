

// Generated on 10/06/2013 17:58:56
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("SpellState", "com.ankamagames.dofus.datacenter.spells")]
    [Serializable]
    public class SpellState : IDataObject, IIndexedData
    {
        private const String MODULE = "SpellStates";
        public int id;
        public uint nameId;
        public Boolean preventsSpellCast;
        public Boolean preventsFight;
        public Boolean critical;
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
        public Boolean PreventsSpellCast
        {
            get { return preventsSpellCast; }
            set { preventsSpellCast = value; }
        }
        [D2OIgnore]
        public Boolean PreventsFight
        {
            get { return preventsFight; }
            set { preventsFight = value; }
        }
        [D2OIgnore]
        public Boolean Critical
        {
            get { return critical; }
            set { critical = value; }
        }
    }
}