
// Generated on 03/02/2013 21:17:47
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("SpellStates")]
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

        public Boolean PreventsSpellCast
        {
            get { return preventsSpellCast; }
            set { preventsSpellCast = value; }
        }

        public Boolean PreventsFight
        {
            get { return preventsFight; }
            set { preventsFight = value; }
        }

        public Boolean Critical
        {
            get { return critical; }
            set { critical = value; }
        }

    }
}