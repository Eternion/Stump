

// Generated on 02/18/2015 11:06:02
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
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public int ChainReactionSpellId
        {
            get { return this.chainReactionSpellId; }
            set { this.chainReactionSpellId = value; }
        }
        [D2OIgnore]
        public int ExplodSpellId
        {
            get { return this.explodSpellId; }
            set { this.explodSpellId = value; }
        }
        [D2OIgnore]
        public int WallId
        {
            get { return this.wallId; }
            set { this.wallId = value; }
        }
        [D2OIgnore]
        public int InstantSpellId
        {
            get { return this.instantSpellId; }
            set { this.instantSpellId = value; }
        }
        [D2OIgnore]
        public int ComboCoeff
        {
            get { return this.comboCoeff; }
            set { this.comboCoeff = value; }
        }
    }
}