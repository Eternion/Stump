 


// Generated on 10/06/2013 14:21:58
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("Challenge")]
    [D2OClass("Challenge")]
    public class ChallengeRecord : ID2ORecord
    {
        private const String MODULE = "Challenge";
        public int id;
        public uint nameId;
        public uint descriptionId;

        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        public uint DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Challenge)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            DescriptionId = castedObj.descriptionId;
        }
        
        public virtual object CreateObject()
        {
            
            var obj = new Challenge();
            obj.id = Id;
            obj.nameId = NameId;
            obj.descriptionId = DescriptionId;
            return obj;
        
        }
    }
}