
// Generated on 03/02/2013 21:17:47
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Spells")]
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

        public uint DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }

        public uint TypeId
        {
            get { return typeId; }
            set { typeId = value; }
        }

        public String ScriptParams
        {
            get { return scriptParams; }
            set { scriptParams = value; }
        }

        public String ScriptParamsCritical
        {
            get { return scriptParamsCritical; }
            set { scriptParamsCritical = value; }
        }

        public int ScriptId
        {
            get { return scriptId; }
            set { scriptId = value; }
        }

        public int ScriptIdCritical
        {
            get { return scriptIdCritical; }
            set { scriptIdCritical = value; }
        }

        public int IconId
        {
            get { return iconId; }
            set { iconId = value; }
        }

        public List<uint> SpellLevels
        {
            get { return spellLevels; }
            set { spellLevels = value; }
        }

        public Boolean UseParamCache
        {
            get { return useParamCache; }
            set { useParamCache = value; }
        }

    }
}