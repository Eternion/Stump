

// Generated on 10/06/2013 17:58:53
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("EffectInstanceDice", "com.ankamagames.dofus.datacenter.effects.instances")]
    [Serializable]
    public class EffectInstanceDice : EffectInstanceInteger
    {
        public uint diceNum;
        public uint diceSide;
        [D2OIgnore]
        public uint DiceNum
        {
            get { return diceNum; }
            set { diceNum = value; }
        }
        [D2OIgnore]
        public uint DiceSide
        {
            get { return diceSide; }
            set { diceSide = value; }
        }
    }
}