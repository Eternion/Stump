 


// Generated on 12/20/2015 18:12:55
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


        [D2OIgnore]
        [PrimaryKey("Id")]
        public int Id
        {
            get;
            set;
        }

        [D2OIgnore]
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

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Comic)obj;
            
            Id = castedObj.id;
            RemoteId = castedObj.remoteId;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (Comic)parent : new Comic();
            obj.id = Id;
            obj.remoteId = RemoteId;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}