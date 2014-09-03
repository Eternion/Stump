

// Generated on 09/02/2014 22:34:32
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
            get { return this.diceNum; }
            set { this.diceNum = value; }
        }
        [D2OIgnore]
        public uint DiceSide
        {
            get { return this.diceSide; }
            set { this.diceSide = value; }
        }
    }
}