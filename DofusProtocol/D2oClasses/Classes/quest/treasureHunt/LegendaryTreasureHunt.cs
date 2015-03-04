

// Generated on 02/19/2015 12:10:49
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("LegendaryTreasureHunt", "com.ankamagames.dofus.datacenter.quest.treasureHunt")]
    [Serializable]
    public class LegendaryTreasureHunt : IDataObject, IIndexedData
    {
        public const String MODULE = "LegendaryTreasureHunts";
        public uint id;
        [I18NField]
        public uint nameId;
        public uint level;
        public uint chestId;
        public uint monsterId;
        public uint mapItemId;
        public double xpRatio;
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public uint Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public uint NameId
        {
            get { return this.nameId; }
            set { this.nameId = value; }
        }
        [D2OIgnore]
        public uint Level
        {
            get { return this.level; }
            set { this.level = value; }
        }
        [D2OIgnore]
        public uint ChestId
        {
            get { return this.chestId; }
            set { this.chestId = value; }
        }
        [D2OIgnore]
        public uint MonsterId
        {
            get { return this.monsterId; }
            set { this.monsterId = value; }
        }
        [D2OIgnore]
        public uint MapItemId
        {
            get { return this.mapItemId; }
            set { this.mapItemId = value; }
        }
        [D2OIgnore]
        public double XpRatio
        {
            get { return this.xpRatio; }
            set { this.xpRatio = value; }
        }
    }
}