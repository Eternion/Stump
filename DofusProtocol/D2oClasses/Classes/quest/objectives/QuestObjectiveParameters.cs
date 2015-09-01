

// Generated on 09/01/2015 11:16:34
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("QuestObjectiveParameters", "com.ankamagames.dofus.datacenter.quest.objectives")]
    [Serializable]
    public class QuestObjectiveParameters : QuestObjective
    {
        public uint numParams;
        public int parameter0;
        public int parameter1;
        public int parameter2;
        public int parameter3;
        public int parameter4;
        public Boolean dungeonOnly;
        [D2OIgnore]
        public uint NumParams
        {
            get { return this.numParams; }
            set { this.numParams = value; }
        }
        [D2OIgnore]
        public int Parameter0
        {
            get { return this.parameter0; }
            set { this.parameter0 = value; }
        }
        [D2OIgnore]
        public int Parameter1
        {
            get { return this.parameter1; }
            set { this.parameter1 = value; }
        }
        [D2OIgnore]
        public int Parameter2
        {
            get { return this.parameter2; }
            set { this.parameter2 = value; }
        }
        [D2OIgnore]
        public int Parameter3
        {
            get { return this.parameter3; }
            set { this.parameter3 = value; }
        }
        [D2OIgnore]
        public int Parameter4
        {
            get { return this.parameter4; }
            set { this.parameter4 = value; }
        }
        [D2OIgnore]
        public Boolean DungeonOnly
        {
            get { return this.dungeonOnly; }
            set { this.dungeonOnly = value; }
        }
    }
}