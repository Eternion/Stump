 


// Generated on 10/19/2013 17:17:44
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
    [TableName("OptionalFeatures")]
    [D2OClass("OptionalFeature", "com.ankamagames.dofus.datacenter.misc")]
    public class OptionalFeatureRecord : ID2ORecord
    {
        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }
        public const String MODULE = "OptionalFeatures";
        public int id;
        public String keyword;

        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        [NullString]
        public String Keyword
        {
            get { return keyword; }
            set { keyword = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (OptionalFeature)obj;
            
            Id = castedObj.id;
            Keyword = castedObj.keyword;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (OptionalFeature)parent : new OptionalFeature();
            obj.id = Id;
            obj.keyword = Keyword;
            return obj;
        
        }
    }
}