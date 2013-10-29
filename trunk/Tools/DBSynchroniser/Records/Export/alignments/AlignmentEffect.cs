 


// Generated on 10/28/2013 14:03:22
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
    [TableName("AlignmentEffect")]
    [D2OClass("AlignmentEffect", "com.ankamagames.dofus.datacenter.alignments")]
    public class AlignmentEffectRecord : ID2ORecord
    {
        private const String MODULE = "AlignmentEffect";
        public int id;
        public uint characteristicId;
        [I18NField]
        public uint descriptionId;

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
        public uint CharacteristicId
        {
            get { return characteristicId; }
            set { characteristicId = value; }
        }

        [D2OIgnore]
        [I18NField]
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
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (AlignmentEffect)parent : new AlignmentEffect();
            obj.id = Id;
            obj.characteristicId = CharacteristicId;
            obj.descriptionId = DescriptionId;
            return obj;
        
        }
    }
}