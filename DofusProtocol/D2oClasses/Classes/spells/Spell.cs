

// Generated on 08/13/2015 17:13:54
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
        public const String MODULE = "Spells";
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
        public Boolean verbose_cast;
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
        public uint DescriptionId
        {
            get { return this.descriptionId; }
            set { this.descriptionId = value; }
        }
        [D2OIgnore]
        public uint TypeId
        {
            get { return this.typeId; }
            set { this.typeId = value; }
        }
        [D2OIgnore]
        public String ScriptParams
        {
            get { return this.scriptParams; }
            set { this.scriptParams = value; }
        }
        [D2OIgnore]
        public String ScriptParamsCritical
        {
            get { return this.scriptParamsCritical; }
            set { this.scriptParamsCritical = value; }
        }
        [D2OIgnore]
        public int ScriptId
        {
            get { return this.scriptId; }
            set { this.scriptId = value; }
        }
        [D2OIgnore]
        public int ScriptIdCritical
        {
            get { return this.scriptIdCritical; }
            set { this.scriptIdCritical = value; }
        }
        [D2OIgnore]
        public int IconId
        {
            get { return this.iconId; }
            set { this.iconId = value; }
        }
        [D2OIgnore]
        public List<uint> SpellLevels
        {
            get { return this.spellLevels; }
            set { this.spellLevels = value; }
        }
        [D2OIgnore]
        public Boolean UseParamCache
        {
            get { return this.useParamCache; }
            set { this.useParamCache = value; }
        }
        [D2OIgnore]
        public Boolean Verbose_cast
        {
            get { return this.verbose_cast; }
            set { this.verbose_cast = value; }
        }
    }
}