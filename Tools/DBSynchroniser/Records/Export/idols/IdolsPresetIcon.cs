 


// Generated on 11/16/2015 14:26:39
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
    [TableName("IdolsPresetIcons")]
    [D2OClass("IdolsPresetIcon", "com.ankamagames.dofus.datacenter.idols")]
    public class IdolsPresetIconRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "IdolsPresetIcons";
        public int id;
        public int order;

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
        public int Order
        {
            get { return order; }
            set { order = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (IdolsPresetIcon)obj;
            
            Id = castedObj.id;
            Order = castedObj.order;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (IdolsPresetIcon)parent : new IdolsPresetIcon();
            obj.id = Id;
            obj.order = Order;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}