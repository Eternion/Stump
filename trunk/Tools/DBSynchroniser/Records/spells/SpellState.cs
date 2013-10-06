 


// Generated on 10/06/2013 14:22:01
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("SpellStates")]
    [D2OClass("SpellState")]
    public class SpellStateRecord : ID2ORecord
    {
        private const String MODULE = "SpellStates";
        public int id;
        public uint nameId;
        public Boolean preventsSpellCast;
        public Boolean preventsFight;
        public Boolean critical;

        [PrimaryKey("Id", false)]
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

        public Boolean PreventsSpellCast
        {
            get { return preventsSpellCast; }
            set { preventsSpellCast = value; }
        }

        public Boolean PreventsFight
        {
            get { return preventsFight; }
            set { preventsFight = value; }
        }

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
        
        public virtual object CreateObject()
        {
            
            var obj = new SpellState();
            obj.id = Id;
            obj.nameId = NameId;
            obj.preventsSpellCast = PreventsSpellCast;
            obj.preventsFight = PreventsFight;
            obj.critical = Critical;
            return obj;
        
        }
    }
}