 


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
    [TableName("AmbientSounds")]
    [D2OClass("AmbientSound", "com.ankamagames.dofus.datacenter.ambientSounds")]
    public class AmbientSoundRecord : ID2ORecord
    {
        public const int AMBIENT_TYPE_ROLEPLAY = 1;
        public const int AMBIENT_TYPE_AMBIENT = 2;
        public const int AMBIENT_TYPE_FIGHT = 3;
        public const int AMBIENT_TYPE_BOSS = 4;
        private const String MODULE = "AmbientSounds";
        public int id;
        public uint volume;
        public int criterionId;
        public uint silenceMin;
        public uint silenceMax;
        public int channel;
        public int type_id;

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
        public uint Volume
        {
            get { return volume; }
            set { volume = value; }
        }

        [D2OIgnore]
        public int CriterionId
        {
            get { return criterionId; }
            set { criterionId = value; }
        }

        [D2OIgnore]
        public uint SilenceMin
        {
            get { return silenceMin; }
            set { silenceMin = value; }
        }

        [D2OIgnore]
        public uint SilenceMax
        {
            get { return silenceMax; }
            set { silenceMax = value; }
        }

        [D2OIgnore]
        public int Channel
        {
            get { return channel; }
            set { channel = value; }
        }

        [D2OIgnore]
        public int Type_id
        {
            get { return type_id; }
            set { type_id = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (AmbientSound)obj;
            
            Id = castedObj.id;
            Volume = castedObj.volume;
            CriterionId = castedObj.criterionId;
            SilenceMin = castedObj.silenceMin;
            SilenceMax = castedObj.silenceMax;
            Channel = castedObj.channel;
            Type_id = castedObj.type_id;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (AmbientSound)parent : new AmbientSound();
            obj.id = Id;
            obj.volume = Volume;
            obj.criterionId = CriterionId;
            obj.silenceMin = SilenceMin;
            obj.silenceMax = SilenceMax;
            obj.channel = Channel;
            obj.type_id = Type_id;
            return obj;
        
        }
    }
}