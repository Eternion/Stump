

// Generated on 02/02/2016 14:15:01
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("HavenbagFurniture", "com.ankamagames.dofus.datacenter.houses")]
    [Serializable]
    public class HavenbagFurniture : IDataObject, IIndexedData
    {
        public const String MODULE = "HavenbagFurnitures";
        public int typeId;
        public int themeId;
        public int elementId;
        public int color;
        public int skillId;
        public int layerId;
        public Boolean blocksMovement;
        public Boolean isStackable;
        public uint cellsWidth;
        public uint cellsHeight;
        public uint order;
        int IIndexedData.Id
        {
            get { return (int)themeId; }
        }
        [D2OIgnore]
        public int TypeId
        {
            get { return this.typeId; }
            set { this.typeId = value; }
        }
        [D2OIgnore]
        public int ThemeId
        {
            get { return this.themeId; }
            set { this.themeId = value; }
        }
        [D2OIgnore]
        public int ElementId
        {
            get { return this.elementId; }
            set { this.elementId = value; }
        }
        [D2OIgnore]
        public int Color
        {
            get { return this.color; }
            set { this.color = value; }
        }
        [D2OIgnore]
        public int SkillId
        {
            get { return this.skillId; }
            set { this.skillId = value; }
        }
        [D2OIgnore]
        public int LayerId
        {
            get { return this.layerId; }
            set { this.layerId = value; }
        }
        [D2OIgnore]
        public Boolean BlocksMovement
        {
            get { return this.blocksMovement; }
            set { this.blocksMovement = value; }
        }
        [D2OIgnore]
        public Boolean IsStackable
        {
            get { return this.isStackable; }
            set { this.isStackable = value; }
        }
        [D2OIgnore]
        public uint CellsWidth
        {
            get { return this.cellsWidth; }
            set { this.cellsWidth = value; }
        }
        [D2OIgnore]
        public uint CellsHeight
        {
            get { return this.cellsHeight; }
            set { this.cellsHeight = value; }
        }
        [D2OIgnore]
        public uint Order
        {
            get { return this.order; }
            set { this.order = value; }
        }
    }
}