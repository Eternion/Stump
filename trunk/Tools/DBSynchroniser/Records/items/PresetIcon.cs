 


// Generated on 10/13/2013 12:21:15
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
    [TableName("PresetIcons")]
    [D2OClass("PresetIcon", "com.ankamagames.dofus.datacenter.items")]
    public class PresetIconRecord : ID2ORecord
    {
        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }
        private const String MODULE = "PresetIcons";
        public int id;
        public int order;

        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        public int Order
        {
            get { return order; }
            set { order = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (PresetIcon)obj;
            
            Id = castedObj.id;
            Order = castedObj.order;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (PresetIcon)parent : new PresetIcon();
            obj.id = Id;
            obj.order = Order;
            return obj;
        
        }
    }
}