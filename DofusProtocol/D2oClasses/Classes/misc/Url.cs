

// Generated on 08/13/2015 17:13:51
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
        public const String MODULE = "Url";
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
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public int BrowserId
        {
            get { return this.browserId; }
            set { this.browserId = value; }
        }
        [D2OIgnore]
        public String Url_
        {
            get { return this.url; }
            set { this.url = value; }
        }
        [D2OIgnore]
        public String Param
        {
            get { return this.param; }
            set { this.param = value; }
        }
        [D2OIgnore]
        public String Method
        {
            get { return this.method; }
            set { this.method = value; }
        }
    }
}