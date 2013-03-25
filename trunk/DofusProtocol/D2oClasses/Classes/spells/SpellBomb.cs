
// Generated on 03/25/2013 19:24:38
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("SpellBombs")]
    [Serializable]
    public class SpellBomb : IDataObject, IIndexedData
    {
        private const String MODULE = "SpellBombs";
        public int id;
        public int chainReactionSpellId;
        public int explodSpellId;
        public int wallId;
        public int instantSpellId;
        public int comboCoeff;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int ChainReactionSpellId
        {
            get { return chainReactionSpellId; }
            set { chainReactionSpellId = value; }
        }

        public int ExplodSpellId
        {
            get { return explodSpellId; }
            set { explodSpellId = value; }
        }

        public int WallId
        {
            get { return wallId; }
            set { wallId = value; }
        }

        public int InstantSpellId
        {
            get { return instantSpellId; }
            set { instantSpellId = value; }
        }

        public int ComboCoeff
        {
            get { return comboCoeff; }
            set { comboCoeff = value; }
        }

    }
}