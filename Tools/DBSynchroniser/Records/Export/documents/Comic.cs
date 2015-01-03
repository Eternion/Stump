 


// Generated on 10/26/2014 23:31:13
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
    [TableName("Comics")]
    [D2OClass("Comic", "com.ankamagames.dofus.datacenter.documents")]
    public class ComicRecord : ID2ORecord, ISaveIntercepter
    {
        private const String MODULE = "Comics";
        public int id;
        public String remoteId;
        public String readerUrl;

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
        [NullString]
        public String RemoteId
        {
            get { return remoteId; }
            set { remoteId = value; }
        }

        [D2OIgnore]
        [NullString]
        public String ReaderUrl
        {
            get { return readerUrl; }
            set { readerUrl = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Comic)obj;
            
            Id = castedObj.id;
            RemoteId = castedObj.remoteId;
            //ReaderUrl = castedObj.readerUrl;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (Comic)parent : new Comic();
            obj.id = Id;
            obj.remoteId = RemoteId;
            //obj.readerUrl = ReaderUrl;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}