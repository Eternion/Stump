

// Generated on 09/26/2016 01:50:30
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
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public int SpellId
        {
            get { return this.spellId; }
            set { this.spellId = value; }
        }
        [D2OIgnore]
        public int CompanionId
        {
            get { return this.companionId; }
            set { this.companionId = value; }
        }
        [D2OIgnore]
        public String GradeByLevel
        {
            get { return this.gradeByLevel; }
            set { this.gradeByLevel = value; }
        }
    }
}