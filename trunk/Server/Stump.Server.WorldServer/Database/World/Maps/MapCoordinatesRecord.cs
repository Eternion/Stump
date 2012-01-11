using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Server.WorldServer.Database.World.Maps
{
    [Serializable]
    [ActiveRecord("maps_coordinates")]
    [D2OClass("MapCoordinates", "com.ankamagames.dofus.datacenter.world")]
    public sealed class MapCoordinateRecord : WorldBaseRecord<MapCoordinateRecord>
    {
        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public int Id
        {
            get;
            set;
        }

       [D2OField("compressedCoords")]
       [Property("CompressedCoords")]
       public uint CompressedCoords
       {
           get;
           set;
       }

       [D2OField("mapIds")]
       [Property("Ids", ColumnType="Serializable")]
       public List<int> Ids
       {
           get;
           set;
       }

    }
}