

// Generated on 10/06/2013 17:58:56
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Spell", "com.ankamagames.dofus.datacenter.spells")]
    [Serializable]
    public class Spell : IDataObject, IIndexedData
    {
        private const String MODULE = "Spells";
        public int id;
        public uint nameId;
        public uint descriptionId;
        public uint typeId;
        public String scriptParams;
        public String scriptParamsCritical;
        public int scriptId;
        public int scriptIdCritical;
        public int iconId;
        public List<uint> spellLevels;
        public Boolean useParamCache = true;
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
        public String ScriptParams
        {
            get { return scriptParams; }
            set { scriptParams = value; }
        }
        [D2OIgnore]
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
        public List<uint> SpellLevels
        {
            get { return spellLevels; }
            set { spellLevels = value; }
        }
        [D2OIgnore]
        public Boolean UseParamCache
        {
            get { return useParamCache; }
            set { useParamCache = value; }
        }
    }
}