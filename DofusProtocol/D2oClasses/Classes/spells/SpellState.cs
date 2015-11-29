

// Generated on 11/16/2015 14:26:35
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
        public Boolean isSilent;
        public Boolean cantDealDamage;
        public Boolean invulnerable;
        public Boolean incurable;
        public Boolean cantBeMoved;
        public Boolean cantBePushed;
        public Boolean cantSwitchPosition;
        public List<int> effectsIds;
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
        [D2OIgnore]
        public Boolean IsSilent
        {
            get { return this.isSilent; }
            set { this.isSilent = value; }
        }
        [D2OIgnore]
        public Boolean CantDealDamage
        {
            get { return this.cantDealDamage; }
            set { this.cantDealDamage = value; }
        }
        [D2OIgnore]
        public Boolean Invulnerable
        {
            get { return this.invulnerable; }
            set { this.invulnerable = value; }
        }
        [D2OIgnore]
        public Boolean Incurable
        {
            get { return this.incurable; }
            set { this.incurable = value; }
        }
        [D2OIgnore]
        public Boolean CantBeMoved
        {
            get { return this.cantBeMoved; }
            set { this.cantBeMoved = value; }
        }
        [D2OIgnore]
        public Boolean CantBePushed
        {
            get { return this.cantBePushed; }
            set { this.cantBePushed = value; }
        }
        [D2OIgnore]
        public Boolean CantSwitchPosition
        {
            get { return this.cantSwitchPosition; }
            set { this.cantSwitchPosition = value; }
        }
        [D2OIgnore]
        public List<int> EffectsIds
        {
            get { return this.effectsIds; }
            set { this.effectsIds = value; }
        }
    }
}