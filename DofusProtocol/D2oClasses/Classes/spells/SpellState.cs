

// Generated on 10/26/2014 23:27:53
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
        public const String MODULE = "SpellStates";
        public int id;
        [I18NField]
        public uint nameId;
        public Boolean preventsSpellCast;
        public Boolean preventsFight;
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
        public uint NameId
        {
            get { return this.nameId; }
            set { this.nameId = value; }
        }
        [D2OIgnore]
        public Boolean PreventsSpellCast
        {
            get { return this.preventsSpellCast; }
            set { this.preventsSpellCast = value; }
        }
        [D2OIgnore]
        public Boolean PreventsFight
        {
            get { return this.preventsFight; }
            set { this.preventsFight = value; }
        }
    }
}