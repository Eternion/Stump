

// Generated on 10/06/2013 17:58:57
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
        private const String MODULE = "WorldMaps";
        public int id;
        public int origineX;
        public int origineY;
        public float mapWidth;
        public float mapHeight;
        public uint horizontalChunck;
        public uint verticalChunck;
        public Boolean viewableEverywhere;
        public float minScale;
        public float maxScale;
        public float startScale;
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
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
        public int OrigineX
        {
            get { return origineX; }
            set { origineX = value; }
        }
        [D2OIgnore]
        public int OrigineY
        {
            get { return origineY; }
            set { origineY = value; }
        }
        [D2OIgnore]
        public float MapWidth
        {
            get { return mapWidth; }
            set { mapWidth = value; }
        }
        [D2OIgnore]
        public float MapHeight
        {
            get { return mapHeight; }
            set { mapHeight = value; }
        }
        [D2OIgnore]
        public uint HorizontalChunck
        {
            get { return horizontalChunck; }
            set { horizontalChunck = value; }
        }
        [D2OIgnore]
        public uint VerticalChunck
        {
            get { return verticalChunck; }
            set { verticalChunck = value; }
        }
        [D2OIgnore]
        public Boolean ViewableEverywhere
        {
            get { return viewableEverywhere; }
            set { viewableEverywhere = value; }
        }
        [D2OIgnore]
        public float MinScale
        {
            get { return minScale; }
            set { minScale = value; }
        }
        [D2OIgnore]
        public float MaxScale
        {
            get { return maxScale; }
            set { maxScale = value; }
        }
        [D2OIgnore]
        public float StartScale
        {
            get { return startScale; }
            set { startScale = value; }
        }
        [D2OIgnore]
        public int CenterX
        {
            get { return centerX; }
            set { centerX = value; }
        }
        [D2OIgnore]
        public int CenterY
        {
            get { return centerY; }
            set { centerY = value; }
        }
        [D2OIgnore]
        public int TotalWidth
        {
            get { return totalWidth; }
            set { totalWidth = value; }
        }
        [D2OIgnore]
        public int TotalHeight
        {
            get { return totalHeight; }
            set { totalHeight = value; }
        }
        [D2OIgnore]
        public List<String> Zoom
        {
            get { return zoom; }
            set { zoom = value; }
        }
    }
}