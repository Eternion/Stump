 


// Generated on 04/19/2016 10:18:05
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
    public class AmbientSoundRecord : PlaylistSoundRecord, ID2ORecord, ISaveIntercepter
    {
        public const int AMBIENT_TYPE_ROLEPLAY = 1;
        public const int AMBIENT_TYPE_AMBIENT = 2;
        public const int AMBIENT_TYPE_FIGHT = 3;
        public const int AMBIENT_TYPE_BOSS = 4;
        public const String MODULE = "AmbientSounds";
        public int criterionId;
        public uint silenceMin;
        public uint silenceMax;
        public int type_id;



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
        public int Type_id
        {
            get { return type_id; }
            set { type_id = value; }
        }

        public override void AssignFields(object obj)
        {
            var castedObj = (AmbientSound)obj;
            
            base.AssignFields(obj);
            CriterionId = castedObj.criterionId;
            SilenceMin = castedObj.silenceMin;
            SilenceMax = castedObj.silenceMax;
            Type_id = castedObj.type_id;
        }
        
        public override object CreateObject(object parent = null)
        {
            var obj = new AmbientSound();
            base.CreateObject(obj);
            obj.criterionId = CriterionId;
            obj.silenceMin = SilenceMin;
            obj.silenceMax = SilenceMax;
            obj.type_id = Type_id;
            return obj;
        }
        
        public override void BeforeSave(bool insert)
        {
            base.BeforeSave(insert);
        
        }
    }
}