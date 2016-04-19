 


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
    [TableName("EmblemBackgrounds")]
    [D2OClass("EmblemBackground", "com.ankamagames.dofus.datacenter.guild")]
    public class EmblemBackgroundRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "EmblemBackgrounds";
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
            var castedObj = (EmblemBackground)obj;
            
            Id = castedObj.id;
            Order = castedObj.order;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (EmblemBackground)parent : new EmblemBackground();
            obj.id = Id;
            obj.order = Order;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}