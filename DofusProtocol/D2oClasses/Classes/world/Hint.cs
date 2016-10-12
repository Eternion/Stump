

// Generated on 09/26/2016 01:50:33
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Hint", "com.ankamagames.dofus.datacenter.world")]
    [Serializable]
    public class Hint : IDataObject, IIndexedData
    {
        public const String MODULE = "Hints";
        public int id;
        public uint gfx;
        [I18NField]
        public uint nameId;
        public uint mapId;
        public uint realMapId;
        [I18NField]
        public int x;
        public int y;
        public Boolean outdoor;
        public int subareaId;
        public int worldMapId;
        public uint level;
        public uint categoryId;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public uint Gfx
        {
            get { return this.gfx; }
            set { this.gfx = value; }
        }
        [D2OIgnore]
        public uint NameId
        {
            get { return this.nameId; }
            set { this.nameId = value; }
        }
        [D2OIgnore]
        public uint MapId
        {
            get { return this.mapId; }
            set { this.mapId = value; }
        }
        [D2OIgnore]
        public uint RealMapId
        {
            get { return this.realMapId; }
            set { this.realMapId = value; }
        }
        [D2OIgnore]
        public int X
        {
            get { return this.x; }
            set { this.x = value; }
        }
        [D2OIgnore]
        public int Y
        {
            get { return this.y; }
            set { this.y = value; }
        }
        [D2OIgnore]
        public Boolean Outdoor
        {
            get { return this.outdoor; }
            set { this.outdoor = value; }
        }
        [D2OIgnore]
        public int SubareaId
        {
            get { return this.subareaId; }
            set { this.subareaId = value; }
        }
        [D2OIgnore]
        public int WorldMapId
        {
            get { return this.worldMapId; }
            set { this.worldMapId = value; }
        }
        [D2OIgnore]
        public uint Level
        {
            get { return this.level; }
            set { this.level = value; }
        }

        [D2OIgnore]
        public uint CategoryId
        {
            get { return this.categoryId; }
            set { this.categoryId = value; }
        }
    }
}