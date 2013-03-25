
// Generated on 03/25/2013 19:24:39
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("WorldMaps")]
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

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int OrigineX
        {
            get { return origineX; }
            set { origineX = value; }
        }

        public int OrigineY
        {
            get { return origineY; }
            set { origineY = value; }
        }

        public float MapWidth
        {
            get { return mapWidth; }
            set { mapWidth = value; }
        }

        public float MapHeight
        {
            get { return mapHeight; }
            set { mapHeight = value; }
        }

        public uint HorizontalChunck
        {
            get { return horizontalChunck; }
            set { horizontalChunck = value; }
        }

        public uint VerticalChunck
        {
            get { return verticalChunck; }
            set { verticalChunck = value; }
        }

        public Boolean ViewableEverywhere
        {
            get { return viewableEverywhere; }
            set { viewableEverywhere = value; }
        }

        public float MinScale
        {
            get { return minScale; }
            set { minScale = value; }
        }

        public float MaxScale
        {
            get { return maxScale; }
            set { maxScale = value; }
        }

        public float StartScale
        {
            get { return startScale; }
            set { startScale = value; }
        }

        public int CenterX
        {
            get { return centerX; }
            set { centerX = value; }
        }

        public int CenterY
        {
            get { return centerY; }
            set { centerY = value; }
        }

        public int TotalWidth
        {
            get { return totalWidth; }
            set { totalWidth = value; }
        }

        public int TotalHeight
        {
            get { return totalHeight; }
            set { totalHeight = value; }
        }

        public List<String> Zoom
        {
            get { return zoom; }
            set { zoom = value; }
        }

    }
}