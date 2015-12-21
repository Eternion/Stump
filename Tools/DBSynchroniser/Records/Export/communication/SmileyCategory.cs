 


// Generated on 12/20/2015 18:16:38
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
    [TableName("SmileyCategories")]
    [D2OClass("SmileyCategory", "com.ankamagames.dofus.datacenter.communication")]
    public class SmileyCategoryRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "SmileyCategories";
        public int id;
        public uint order;
        public String gfxId;
        public Boolean isFake;

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
        public uint Order
        {
            get { return order; }
            set { order = value; }
        }

        [D2OIgnore]
        [NullString]
        public String GfxId
        {
            get { return gfxId; }
            set { gfxId = value; }
        }

        [D2OIgnore]
        public Boolean IsFake
        {
            get { return isFake; }
            set { isFake = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (SmileyCategory)obj;
            
            Id = castedObj.id;
            Order = castedObj.order;
            GfxId = castedObj.gfxId;
            IsFake = castedObj.isFake;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (SmileyCategory)parent : new SmileyCategory();
            obj.id = Id;
            obj.order = Order;
            obj.gfxId = GfxId;
            obj.isFake = IsFake;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}