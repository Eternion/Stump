

// Generated on 12/12/2013 16:57:41
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("CompanionSpell", "com.ankamagames.dofus.datacenter.monsters")]
    [Serializable]
    public class CompanionSpell : IDataObject, IIndexedData
    {
        public const String MODULE = "CompanionSpells";
        public int id;
        public int spellId;
        public int companionId;
        public String gradeByLevel;
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
        public int SpellId
        {
            get { return spellId; }
            set { spellId = value; }
        }
        [D2OIgnore]
        public int CompanionId
        {
            get { return companionId; }
            set { companionId = value; }
        }
        [D2OIgnore]
        public String GradeByLevel
        {
            get { return gradeByLevel; }
            set { gradeByLevel = value; }
        }
    }
}