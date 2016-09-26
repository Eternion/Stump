

// Generated on 09/26/2016 01:50:34
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("WorldMap", "com.ankamagames.dofus.datacenter.world")]
    [Serializable]
    public class WorldMap : IDataObject, IIndexedData
    {
        public const String MODULE = "WorldMaps";
        public int id;
        [I18NField]
        public uint nameId;
        public int origineX;
        public int origineY;
        public double mapWidth;
        public double mapHeight;
        public uint horizontalChunck;
        public uint verticalChunck;
        public Boolean viewableEverywhere;
        public double minScale;
        public double maxScale;
        public double startScale;
        public int centerX;
        public int centerY;
        public int totalWidth;
        public int totalHeight;
        public List<String> zoom;
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
        public uint NameId
        {
            get { return this.nameId; }
            set { this.nameId = value; }
        }
        [D2OIgnore]
        public int OrigineX
        {
            get { return this.origineX; }
            set { this.origineX = value; }
        }
        [D2OIgnore]
        public int OrigineY
        {
            get { return this.origineY; }
            set { this.origineY = value; }
        }
        [D2OIgnore]
        public double MapWidth
        {
            get { return this.mapWidth; }
            set { this.mapWidth = value; }
        }
        [D2OIgnore]
        public double MapHeight
        {
            get { return this.mapHeight; }
            set { this.mapHeight = value; }
        }
        [D2OIgnore]
        public uint HorizontalChunck
        {
            get { return this.horizontalChunck; }
            set { this.horizontalChunck = value; }
        }
        [D2OIgnore]
        public uint VerticalChunck
        {
            get { return this.verticalChunck; }
            set { this.verticalChunck = value; }
        }
        [D2OIgnore]
        public Boolean ViewableEverywhere
        {
            get { return this.viewableEverywhere; }
            set { this.viewableEverywhere = value; }
        }
        [D2OIgnore]
        public double MinScale
        {
            get { return this.minScale; }
            set { this.minScale = value; }
        }
        [D2OIgnore]
        public double MaxScale
        {
            get { return this.maxScale; }
            set { this.maxScale = value; }
        }
        [D2OIgnore]
        public double StartScale
        {
            get { return this.startScale; }
            set { this.startScale = value; }
        }
        [D2OIgnore]
        public int CenterX
        {
            get { return this.centerX; }
            set { this.centerX = value; }
        }
        [D2OIgnore]
        public int CenterY
        {
            get { return this.centerY; }
            set { this.centerY = value; }
        }
        [D2OIgnore]
        public int TotalWidth
        {
            get { return this.totalWidth; }
            set { this.totalWidth = value; }
        }
        [D2OIgnore]
        public int TotalHeight
        {
            get { return this.totalHeight; }
            set { this.totalHeight = value; }
        }
        [D2OIgnore]
        public List<String> Zoom
        {
            get { return this.zoom; }
            set { this.zoom = value; }
        }
    }
}