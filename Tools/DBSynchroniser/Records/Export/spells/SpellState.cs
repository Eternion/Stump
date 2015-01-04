 


// Generated on 01/04/2015 01:23:49
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

        public virtual void AssignFields(object obj)
        {
            var castedObj = (SpellState)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            PreventsSpellCast = castedObj.preventsSpellCast;
            PreventsFight = castedObj.preventsFight;
            IsSilent = castedObj.isSilent;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (SpellState)parent : new SpellState();
            obj.id = Id;
            obj.nameId = NameId;
            obj.preventsSpellCast = PreventsSpellCast;
            obj.preventsFight = PreventsFight;
            obj.isSilent = IsSilent;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}