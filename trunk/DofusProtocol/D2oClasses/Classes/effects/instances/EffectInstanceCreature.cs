

// Generated on 10/28/2013 14:03:17
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
            get { return monsterFamilyId; }
            set { monsterFamilyId = value; }
        }
    }
}