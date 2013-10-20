 


// Generated on 10/19/2013 17:17:46
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
    public class SpellStateRecord : ID2ORecord
    {
        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }
        private const String MODULE = "SpellStates";
        public int id;
        public uint nameId;
        public Boolean preventsSpellCast;
        public Boolean preventsFight;
        public Boolean critical;

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
        public Boolean Critical
        {
            get { return critical; }
            set { critical = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (SpellState)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            PreventsSpellCast = castedObj.preventsSpellCast;
            PreventsFight = castedObj.preventsFight;
            Critical = castedObj.critical;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (SpellState)parent : new SpellState();
            obj.id = Id;
            obj.nameId = NameId;
            obj.preventsSpellCast = PreventsSpellCast;
            obj.preventsFight = PreventsFight;
            obj.critical = Critical;
            return obj;
        
        }
    }
}