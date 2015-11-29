 


// Generated on 11/16/2015 14:26:43
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("SpellStates")]
    [D2OClass("SpellState", "com.ankamagames.dofus.datacenter.spells")]
    public class SpellStateRecord : ID2ORecord, ISaveIntercepter
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

        int ID2ORecord.Id
        {
            get { return (int)id; }
        }


        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        [I18NField]
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
        public Boolean IsSilent
        {
            get { return isSilent; }
            set { isSilent = value; }
        }

        [D2OIgnore]
        public Boolean CantDealDamage
        {
            get { return cantDealDamage; }
            set { cantDealDamage = value; }
        }

        [D2OIgnore]
        public Boolean Invulnerable
        {
            get { return invulnerable; }
            set { invulnerable = value; }
        }

        [D2OIgnore]
        public Boolean Incurable
        {
            get { return incurable; }
            set { incurable = value; }
        }

        [D2OIgnore]
        public Boolean CantBeMoved
        {
            get { return cantBeMoved; }
            set { cantBeMoved = value; }
        }

        [D2OIgnore]
        public Boolean CantBePushed
        {
            get { return cantBePushed; }
            set { cantBePushed = value; }
        }

        [D2OIgnore]
        public Boolean CantSwitchPosition
        {
            get { return cantSwitchPosition; }
            set { cantSwitchPosition = value; }
        }

        [D2OIgnore]
        [Ignore]
        public List<int> EffectsIds
        {
            get { return effectsIds; }
            set
            {
                effectsIds = value;
                m_effectsIdsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_effectsIdsBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] EffectsIdsBin
        {
            get { return m_effectsIdsBin; }
            set
            {
                m_effectsIdsBin = value;
                effectsIds = value == null ? null : value.ToObject<List<int>>();
            }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (SpellState)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            PreventsSpellCast = castedObj.preventsSpellCast;
            PreventsFight = castedObj.preventsFight;
            IsSilent = castedObj.isSilent;
            CantDealDamage = castedObj.cantDealDamage;
            Invulnerable = castedObj.invulnerable;
            Incurable = castedObj.incurable;
            CantBeMoved = castedObj.cantBeMoved;
            CantBePushed = castedObj.cantBePushed;
            CantSwitchPosition = castedObj.cantSwitchPosition;
            EffectsIds = castedObj.effectsIds;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (SpellState)parent : new SpellState();
            obj.id = Id;
            obj.nameId = NameId;
            obj.preventsSpellCast = PreventsSpellCast;
            obj.preventsFight = PreventsFight;
            obj.isSilent = IsSilent;
            obj.cantDealDamage = CantDealDamage;
            obj.invulnerable = Invulnerable;
            obj.incurable = Incurable;
            obj.cantBeMoved = CantBeMoved;
            obj.cantBePushed = CantBePushed;
            obj.cantSwitchPosition = CantSwitchPosition;
            obj.effectsIds = EffectsIds;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
            m_effectsIdsBin = effectsIds == null ? null : effectsIds.ToBinary();
        
        }
    }
}