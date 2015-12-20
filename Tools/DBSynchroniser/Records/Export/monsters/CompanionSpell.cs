 


// Generated on 12/20/2015 18:16:41
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
    [TableName("CompanionSpells")]
    [D2OClass("CompanionSpell", "com.ankamagames.dofus.datacenter.monsters")]
    public class CompanionSpellRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "CompanionSpells";
        public int id;
        public int spellId;
        public int companionId;
        public String gradeByLevel;

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
        [NullString]
        public String GradeByLevel
        {
            get { return gradeByLevel; }
            set { gradeByLevel = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (CompanionSpell)obj;
            
            Id = castedObj.id;
            SpellId = castedObj.spellId;
            CompanionId = castedObj.companionId;
            GradeByLevel = castedObj.gradeByLevel;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (CompanionSpell)parent : new CompanionSpell();
            obj.id = Id;
            obj.spellId = SpellId;
            obj.companionId = CompanionId;
            obj.gradeByLevel = GradeByLevel;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}