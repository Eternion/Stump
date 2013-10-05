 


// Generated on 10/06/2013 01:10:59
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [D2OClass("Url")]
    public class UrlRecord : ID2ORecord
    {
        private const String MODULE = "Url";
        public int id;
        public int browserId;
        public String url;
        public String param;
        public String method;

        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int BrowserId
        {
            get { return browserId; }
            set { browserId = value; }
        }

        public String Url
        {
            get { return url; }
            set { url = value; }
        }

        public String Param
        {
            get { return param; }
            set { param = value; }
        }

        public String Method
        {
            get { return method; }
            set { method = value; }
        }

        public void AssignFields(object obj)
        {
            var castedObj = (Url)obj;
            
            Id = castedObj.id;
            BrowserId = castedObj.browserId;
            Url = castedObj.url;
            Param = castedObj.param;
            Method = castedObj.method;
        }
        
        public object CreateObject()
        {
            var obj = new Url();
            
            obj.id = Id;
            obj.browserId = BrowserId;
            obj.url = Url;
            obj.param = Param;
            obj.method = Method;
            return obj;
        
        }
    }
}