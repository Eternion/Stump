

// Generated on 04/24/2015 03:38:25
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("EffectInstanceLadder", "com.ankamagames.dofus.datacenter.effects.instances")]
    [Serializable]
    public class EffectInstanceLadder : EffectInstanceCreature
    {
        public uint monsterCount;
        [D2OIgnore]
        public uint MonsterCount
        {
            get { return this.monsterCount; }
            set { this.monsterCount = value; }
        }
    }
}