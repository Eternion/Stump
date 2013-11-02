 


// Generated on 11/02/2013 14:55:50
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
    [TableName("Spells")]
    [D2OClass("Spell", "com.ankamagames.dofus.datacenter.spells")]
    public class SpellRecord : ID2ORecord, ISaveIntercepter
    {
        private const String MODULE = "Spells";
        public int id;
        [I18NField]
        public uint nameId;
        [I18NField]
        public uint descriptionId;
        public uint typeId;
        public String scriptParams;
        public String scriptParamsCritical;
        public int scriptId;
        public int scriptIdCritical;
        public int iconId;
        public List<uint> spellLevels;
        public Boolean useParamCache = true;

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
        [I18NField]
        public uint DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }

        [D2OIgnore]
        public uint TypeId
        {
            get { return typeId; }
            set { typeId = value; }
        }

        [D2OIgnore]
        [NullString]
        public String ScriptParams
        {
            get { return scriptParams; }
            set { scriptParams = value; }
        }

        [D2OIgnore]
        [NullString]
        public String ScriptParamsCritical
        {
            get { return scriptParamsCritical; }
            set { scriptParamsCritical = value; }
        }

        [D2OIgnore]
        public int ScriptId
        {
            get { return scriptId; }
            set { scriptId = value; }
        }

        [D2OIgnore]
        public int ScriptIdCritical
        {
            get { return scriptIdCritical; }
            set { scriptIdCritical = value; }
        }

        [D2OIgnore]
        public int IconId
        {
            get { return iconId; }
            set { iconId = value; }
        }

        [D2OIgnore]
        [Ignore]
        public List<uint> SpellLevels
        {
            get { return spellLevels; }
            set
            {
                spellLevels = value;
                m_spellLevelsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_spellLevelsBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] SpellLevelsBin
        {
            get { return m_spellLevelsBin; }
            set
            {
                m_spellLevelsBin = value;
                spellLevels = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        [D2OIgnore]
        public Boolean UseParamCache
        {
            get { return useParamCache; }
            set { useParamCache = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Spell)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            DescriptionId = castedObj.descriptionId;
            TypeId = castedObj.typeId;
            ScriptParams = castedObj.scriptParams;
            ScriptParamsCritical = castedObj.scriptParamsCritical;
            ScriptId = castedObj.scriptId;
            ScriptIdCritical = castedObj.scriptIdCritical;
            IconId = castedObj.iconId;
            SpellLevels = castedObj.spellLevels;
            UseParamCache = castedObj.useParamCache;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (Spell)parent : new Spell();
            obj.id = Id;
            obj.nameId = NameId;
            obj.descriptionId = DescriptionId;
            obj.typeId = TypeId;
            obj.scriptParams = ScriptParams;
            obj.scriptParamsCritical = ScriptParamsCritical;
            obj.scriptId = ScriptId;
            obj.scriptIdCritical = ScriptIdCritical;
            obj.iconId = IconId;
            obj.spellLevels = SpellLevels;
            obj.useParamCache = UseParamCache;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
            m_spellLevelsBin = spellLevels == null ? null : spellLevels.ToBinary();
        
        }
    }
}