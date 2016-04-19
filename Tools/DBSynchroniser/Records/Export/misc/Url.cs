 


// Generated on 04/19/2016 10:18:09
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
    [TableName("Url")]
    [D2OClass("Url", "com.ankamagames.dofus.datacenter.misc")]
    public class UrlRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "Url";
        public int id;
        public int browserId;
        public String url;
        public String param;
        public String method;

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
        public int BrowserId
        {
            get { return browserId; }
            set { browserId = value; }
        }

        [D2OIgnore]
        [NullString]
        public String Url
        {
            get { return url; }
            set { url = value; }
        }

        [D2OIgnore]
        [NullString]
        public String Param
        {
            get { return param; }
            set { param = value; }
        }

        [D2OIgnore]
        [NullString]
        public String Method
        {
            get { return method; }
            set { method = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Url)obj;
            
            Id = castedObj.id;
            BrowserId = castedObj.browserId;
            Url = castedObj.url;
            Param = castedObj.param;
            Method = castedObj.method;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (Url)parent : new Url();
            obj.id = Id;
            obj.browserId = BrowserId;
            obj.url = Url;
            obj.param = Param;
            obj.method = Method;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}