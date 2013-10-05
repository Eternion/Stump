 


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
    [D2OClass("OptionalFeatures")]
    public class OptionalFeatureRecord : ID2ORecord
    {
        public const String MODULE = "OptionalFeatures";
        public int id;
        public String keyword;

        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public String Keyword
        {
            get { return keyword; }
            set { keyword = value; }
        }

        public void AssignFields(object obj)
        {
            var castedObj = (OptionalFeature)obj;
            
            Id = castedObj.id;
            Keyword = castedObj.keyword;
        }
        
        public object CreateObject()
        {
            var obj = new OptionalFeature();
            
            obj.id = Id;
            obj.keyword = Keyword;
            return obj;
        
        }
    }
}