 


// Generated on 10/06/2013 14:21:59
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("Interactives")]
    [D2OClass("Interactive")]
    public class InteractiveRecord : ID2ORecord
    {
        private const String MODULE = "Interactives";
        public int id;
        public uint nameId;
        public int actionId;
        public Boolean displayTooltip;

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

        public int ActionId
        {
            get { return actionId; }
            set { actionId = value; }
        }

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
        
        public virtual object CreateObject()
        {
            
            var obj = new Interactive();
            obj.id = Id;
            obj.nameId = NameId;
            obj.actionId = ActionId;
            obj.displayTooltip = DisplayTooltip;
            return obj;
        
        }
    }
}