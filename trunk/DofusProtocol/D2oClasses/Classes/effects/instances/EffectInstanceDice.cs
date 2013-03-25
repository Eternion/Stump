
// Generated on 03/25/2013 19:24:33
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("EffectInstanceDice")]
    [Serializable]
    public class EffectInstanceDice : EffectInstanceInteger, IIndexedData
    {
        public uint diceNum;
        public uint diceSide;

        public uint DiceNum
        {
            get { return diceNum; }
            set { diceNum = value; }
        }

        public uint DiceSide
        {
            get { return diceSide; }
            set { diceSide = value; }
        }

    }
}