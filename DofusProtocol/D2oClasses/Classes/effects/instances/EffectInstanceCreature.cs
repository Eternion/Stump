

// Generated on 10/26/2014 23:27:35
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("EffectInstanceCreature", "com.ankamagames.dofus.datacenter.effects.instances")]
    [Serializable]
    public class EffectInstanceCreature : EffectInstance
    {
        public uint monsterFamilyId;
        [D2OIgnore]
        public uint MonsterFamilyId
        {
            get { return this.monsterFamilyId; }
            set { this.monsterFamilyId = value; }
        }
    }
}