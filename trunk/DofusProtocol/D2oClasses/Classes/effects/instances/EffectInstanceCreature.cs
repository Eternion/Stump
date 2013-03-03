
// Generated on 03/02/2013 21:17:44
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("EffectInstanceCreature")]
    [Serializable]
    public class EffectInstanceCreature : EffectInstance, IIndexedData
    {
        public uint monsterFamilyId;

        public uint MonsterFamilyId
        {
            get { return monsterFamilyId; }
            set { monsterFamilyId = value; }
        }

    }
}