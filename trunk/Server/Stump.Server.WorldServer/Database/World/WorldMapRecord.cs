using System;
using System.Data.Entity.ModelConfiguration;
using Stump.DofusProtocol.D2oClasses;

namespace Stump.Server.WorldServer.Database
{
    public class WorldMapRecordConfiguration : EntityTypeConfiguration<WorldMapRecord>
    {
        public WorldMapRecordConfiguration()
        {
            ToTable("worlds_maps");
        }
    }

    [D2OClass("WorldMap", "com.ankamagames.dofus.datacenter.world")]
    public sealed class WorldMapRecord : IAssignedByD2O
    {
        public int Id
        {
            get;
            set;
        }

        public int OrigineX
        {
            get;
            set;
        }

        public int OrigineY
        {
            get;
            set;
        }

        public double MapWidth
        {
            get;
            set;
        }

        public double MapHeight
        {
            get;
            set;
        }

        public uint HorizontalChunck
        {
            get;
            set;
        }

        public uint VerticalChunck
        {
            get;
            set;
        }

        public Boolean ViewableEverywhere
        {
            get;
            set;
        }

        public double MinScale
        {
            get;
            set;
        }

        public double MaxScale
        {
            get;
            set;
        }

        public double StartScale
        {
            get;
            set;
        }

        public int CenterX
        {
            get;
            set;
        }

        public int CenterY
        {
            get;
            set;
        }

        public int TotalWidth
        {
            get;
            set;
        }

        public int TotalHeight
        {
            get;
            set;
        }

        #region IAssignedByD2O Members

        public void AssignFields(object d2oObject)
        {
            var map = (DofusProtocol.D2oClasses.WorldMap) d2oObject;
            Id = map.id;
            OrigineX = map.origineX;
            OrigineY = map.origineY;

            MapWidth = map.mapWidth;
            MapHeight = map.mapHeight;
            HorizontalChunck = map.horizontalChunck;
            VerticalChunck = map.verticalChunck;
            ViewableEverywhere = map.viewableEverywhere;
            MinScale = map.minScale;
            MaxScale = map.maxScale;
            StartScale = map.startScale;
            CenterX = map.centerX;
            CenterY = map.centerY;
            TotalWidth = map.totalWidth;
            TotalHeight = map.totalHeight;
        }

        #endregion
    }
}