 


// Generated on 04/19/2016 10:18:07
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
    [TableName("Interactives")]
    [D2OClass("Interactive", "com.ankamagames.dofus.datacenter.interactives")]
    public class InteractiveRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "Interactives";
        public int id;
        [I18NField]
        public uint nameId;
        public int actionId;
        public Boolean displayTooltip;

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
        public int ActionId
        {
            get { return actionId; }
            set { actionId = value; }
        }

        [D2OIgnore]
        public Boolean DisplayTooltip
        {
            get { return displayTooltip; }
            set { displayTooltip = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Interactive)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            ActionId = castedObj.actionId;
            DisplayTooltip = castedObj.displayTooltip;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (Interactive)parent : new Interactive();
            obj.id = Id;
            obj.nameId = NameId;
            obj.actionId = ActionId;
            obj.displayTooltip = DisplayTooltip;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}