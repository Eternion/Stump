 


// Generated on 10/06/2013 14:21:57
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("AlignmentEffect")]
    [D2OClass("AlignmentEffect")]
    public class AlignmentEffectRecord : ID2ORecord
    {
        private const String MODULE = "AlignmentEffect";
        public int id;
        public uint characteristicId;
        public uint descriptionId;

        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint CharacteristicId
        {
            get { return characteristicId; }
            set { characteristicId = value; }
        }

        public uint DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (AlignmentEffect)obj;
            
            Id = castedObj.id;
            CharacteristicId = castedObj.characteristicId;
            DescriptionId = castedObj.descriptionId;
        }
        
        public virtual object CreateObject()
        {
            
            var obj = new AlignmentEffect();
            obj.id = Id;
            obj.characteristicId = CharacteristicId;
            obj.descriptionId = DescriptionId;
            return obj;
        
        }
    }
}