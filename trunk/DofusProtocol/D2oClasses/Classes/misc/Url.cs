
// Generated on 03/02/2013 21:17:46
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Url")]
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

        public String _Url
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

    }
}