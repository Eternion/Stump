

// Generated on 12/12/2013 16:57:43
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("SpellBomb", "com.ankamagames.dofus.datacenter.spells")]
    [Serializable]
    public class SpellBomb : IDataObject, IIndexedData
    {
        public const String MODULE = "SpellBombs";
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
        [D2OIgnore]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
        public int ChainReactionSpellId
        {
            get { return chainReactionSpellId; }
            set { chainReactionSpellId = value; }
        }
        [D2OIgnore]
        public int ExplodSpellId
        {
            get { return explodSpellId; }
            set { explodSpellId = value; }
        }
        [D2OIgnore]
        public int WallId
        {
            get { return wallId; }
            set { wallId = value; }
        }
        [D2OIgnore]
        public int InstantSpellId
        {
            get { return instantSpellId; }
            set { instantSpellId = value; }
        }
        [D2OIgnore]
        public int ComboCoeff
        {
            get { return comboCoeff; }
            set { comboCoeff = value; }
        }
    }
}