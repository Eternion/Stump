 


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
    [TableName("AmbientSounds")]
    [D2OClass("AmbientSound")]
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

        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint Volume
        {
            get { return volume; }
            set { volume = value; }
        }

        public int CriterionId
        {
            get { return criterionId; }
            set { criterionId = value; }
        }

        public uint SilenceMin
        {
            get { return silenceMin; }
            set { silenceMin = value; }
        }

        public uint SilenceMax
        {
            get { return silenceMax; }
            set { silenceMax = value; }
        }

        public int Channel
        {
            get { return channel; }
            set { channel = value; }
        }

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
        
        public virtual object CreateObject()
        {
            
            var obj = new AmbientSound();
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