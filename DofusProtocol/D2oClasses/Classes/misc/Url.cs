

// Generated on 10/28/2013 14:03:19
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Url", "com.ankamagames.dofus.datacenter.misc")]
    [Serializable]
    public class Url : IDataObject, IIndexedData
    {
        private const String MODULE = "Url";
        public int id;
        public int browserId;
        public String url;
        public String param;
        public String method;
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
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
        public String Url_
        {
            get { return url; }
            set { url = value; }
        }
        [D2OIgnore]
        public String Param
        {
            get { return param; }
            set { param = value; }
        }
        [D2OIgnore]
        public String Method
        {
            get { return method; }
            set { method = value; }
        }
    }
}