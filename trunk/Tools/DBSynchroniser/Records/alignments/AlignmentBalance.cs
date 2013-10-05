 


// Generated on 10/06/2013 01:10:57
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [D2OClass("AlignmentBalance")]
    public class AlignmentBalanceRecord : ID2ORecord
    {
        private const String MODULE = "AlignmentBalance";
        public int id;
        public int startValue;
        public int endValue;
        public uint nameId;
        public uint descriptionId;

        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int StartValue
        {
            get { return startValue; }
            set { startValue = value; }
        }

        public int EndValue
        {
            get { return endValue; }
            set { endValue = value; }
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

        public void AssignFields(object obj)
        {
            var castedObj = (AlignmentBalance)obj;
            
            Id = castedObj.id;
            StartValue = castedObj.startValue;
            EndValue = castedObj.endValue;
            NameId = castedObj.nameId;
            DescriptionId = castedObj.descriptionId;
        }
        
        public object CreateObject()
        {
            var obj = new AlignmentBalance();
            
            obj.id = Id;
            obj.startValue = StartValue;
            obj.endValue = EndValue;
            obj.nameId = NameId;
            obj.descriptionId = DescriptionId;
            return obj;
        
        }
    }
}