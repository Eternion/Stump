 


// Generated on 10/06/2013 01:11:01
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [D2OClass("WorldMaps")]
    public class WorldMapRecord : ID2ORecord
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

        [PrimaryKey("Id", false)]
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

        [Ignore]
        public List<String> Zoom
        {
            get { return zoom; }
            set
            {
                zoom = value;
                m_zoomBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_zoomBin;
        public byte[] ZoomBin
        {
            get { return m_zoomBin; }
            set
            {
                m_zoomBin = value;
                zoom = value == null ? null : value.ToObject<List<String>>();
            }
        }

        public void AssignFields(object obj)
        {
            var castedObj = (WorldMap)obj;
            
            Id = castedObj.id;
            OrigineX = castedObj.origineX;
            OrigineY = castedObj.origineY;
            MapWidth = castedObj.mapWidth;
            MapHeight = castedObj.mapHeight;
            HorizontalChunck = castedObj.horizontalChunck;
            VerticalChunck = castedObj.verticalChunck;
            ViewableEverywhere = castedObj.viewableEverywhere;
            MinScale = castedObj.minScale;
            MaxScale = castedObj.maxScale;
            StartScale = castedObj.startScale;
            CenterX = castedObj.centerX;
            CenterY = castedObj.centerY;
            TotalWidth = castedObj.totalWidth;
            TotalHeight = castedObj.totalHeight;
            Zoom = castedObj.zoom;
        }
        
        public object CreateObject()
        {
            var obj = new WorldMap();
            
            obj.id = Id;
            obj.origineX = OrigineX;
            obj.origineY = OrigineY;
            obj.mapWidth = MapWidth;
            obj.mapHeight = MapHeight;
            obj.horizontalChunck = HorizontalChunck;
            obj.verticalChunck = VerticalChunck;
            obj.viewableEverywhere = ViewableEverywhere;
            obj.minScale = MinScale;
            obj.maxScale = MaxScale;
            obj.startScale = StartScale;
            obj.centerX = CenterX;
            obj.centerY = CenterY;
            obj.totalWidth = TotalWidth;
            obj.totalHeight = TotalHeight;
            obj.zoom = Zoom;
            return obj;
        
        }
    }
}