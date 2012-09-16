using System.Data.Entity.ModelConfiguration;
using Stump.DofusProtocol.D2oClasses;

namespace Stump.Server.WorldServer.Database.Maps
{
    public class MapReferenceRecordConfiguration : EntityTypeConfiguration<MapReferenceRecord>
    {
        public MapReferenceRecordConfiguration()
        {
            ToTable("maps_references");
        }
    }

    [D2OClass("MapReference", "com.ankamagames.dofus.datacenter.world")]
    public sealed class MapReferenceRecord : IAssignedByD2O
    {

       public int Id
       {
           get;
           set;
       }

       public uint MapId
       {
           get;
           set;
       }

       public int CellId
       {
           get;
           set;
       }

        public void AssignFields(object d2oObject)
        {
            var map = (DofusProtocol.D2oClasses.MapReference)d2oObject;
            Id = map.id;
            MapId = map.mapId;
            CellId = map.cellId;
        }
    }
}