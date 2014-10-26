

// Generated on 10/26/2014 23:27:35
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Comic", "com.ankamagames.dofus.datacenter.documents")]
    [Serializable]
    public class Comic : IDataObject, IIndexedData
    {
        private const String MODULE = "Comics";
        public int id;
        public String remoteId;
        public String readerUrl;
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
        public String RemoteId
        {
            get { return this.remoteId; }
            set { this.remoteId = value; }
        }
        [D2OIgnore]
        public String ReaderUrl
        {
            get { return this.readerUrl; }
            set { this.readerUrl = value; }
        }
    }
}