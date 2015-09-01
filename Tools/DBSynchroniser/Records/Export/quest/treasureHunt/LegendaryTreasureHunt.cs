 


// Generated on 09/01/2015 10:48:50
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("LegendaryTreasureHunts")]
    [D2OClass("LegendaryTreasureHunt", "com.ankamagames.dofus.datacenter.quest.treasureHunt")]
    public class LegendaryTreasureHuntRecord : ID2ORecord, ISaveIntercepter
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

        int ID2ORecord.Id
        {
            get { return (int)id; }
        }


        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        [I18NField]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        [D2OIgnore]
        public uint Level
        {
            get { return level; }
            set { level = value; }
        }

        [D2OIgnore]
        public uint ChestId
        {
            get { return chestId; }
            set { chestId = value; }
        }

        [D2OIgnore]
        public uint MonsterId
        {
            get { return monsterId; }
            set { monsterId = value; }
        }

        [D2OIgnore]
        public uint MapItemId
        {
            get { return mapItemId; }
            set { mapItemId = value; }
        }

        [D2OIgnore]
        public double XpRatio
        {
            get { return xpRatio; }
            set { xpRatio = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (LegendaryTreasureHunt)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            Level = castedObj.level;
            ChestId = castedObj.chestId;
            MonsterId = castedObj.monsterId;
            MapItemId = castedObj.mapItemId;
            XpRatio = castedObj.xpRatio;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (LegendaryTreasureHunt)parent : new LegendaryTreasureHunt();
            obj.id = Id;
            obj.nameId = NameId;
            obj.level = Level;
            obj.chestId = ChestId;
            obj.monsterId = MonsterId;
            obj.mapItemId = MapItemId;
            obj.xpRatio = XpRatio;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}