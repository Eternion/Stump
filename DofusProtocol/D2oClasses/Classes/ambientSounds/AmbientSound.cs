

// Generated on 02/02/2016 14:14:59
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("AmbientSound", "com.ankamagames.dofus.datacenter.ambientSounds")]
    [Serializable]
    public class AmbientSound : PlaylistSound
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
            get { return this.criterionId; }
            set { this.criterionId = value; }
        }
        [D2OIgnore]
        public uint SilenceMin
        {
            get { return this.silenceMin; }
            set { this.silenceMin = value; }
        }
        [D2OIgnore]
        public uint SilenceMax
        {
            get { return this.silenceMax; }
            set { this.silenceMax = value; }
        }
        [D2OIgnore]
        public int Type_id
        {
            get { return this.type_id; }
            set { this.type_id = value; }
        }
    }
}