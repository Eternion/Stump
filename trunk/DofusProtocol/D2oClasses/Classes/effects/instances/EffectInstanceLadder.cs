
// Generated on 03/02/2013 21:17:44
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("EffectInstanceLadder")]
    [Serializable]
    public class EffectInstanceLadder : EffectInstanceCreature, IIndexedData
    {
        public uint monsterCount;

        public uint MonsterCount
        {
            get { return monsterCount; }
            set { monsterCount = value; }
        }

    }
}