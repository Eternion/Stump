using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Objects;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.ORM;
using Stump.Server.BaseServer.Database;

namespace Stump.Server.WorldServer.Database.Maps
{
    public class MapCoordinatesRecordConfiguration : EntityTypeConfiguration<MapCoordinateRecord>
    {
        public MapCoordinatesRecordConfiguration()
        {
            ToTable("maps_coordinates");
        }
    }

    [D2OClass("MapCoordinates", "com.ankamagames.dofus.datacenter.world")]
    public sealed class MapCoordinateRecord : IAssignedByD2O, ISaveIntercepter
    {
        private byte[] m_mapIdsBin;

        public int Id
        {
            get;
            set;
        }

        public uint CompressedCoords
        {
            get;
            set;
        }

        public byte[] MapIdsBin
        {
            get { return m_mapIdsBin; }
            set
            {
                m_mapIdsBin = value;
                MapIds = value.ToObject<List<int>>();
            }
        }

        public List<int> MapIds
        {
            get;
            set;
        }

        #region IAssignedByD2O Members

        public void AssignFields(object d2oObject)
        {
            var map = (DofusProtocol.D2oClasses.MapCoordinates) d2oObject;
            CompressedCoords = map.compressedCoords;
            MapIds = map.mapIds;
        }

        #endregion

        #region ISaveIntercepter Members

        public void BeforeSave(ObjectStateEntry currentEntry)
        {
            m_mapIdsBin = MapIds.ToBinary();
        }

        #endregion
    }
}