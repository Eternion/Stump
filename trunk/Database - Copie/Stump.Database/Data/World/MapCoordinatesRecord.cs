using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.World
{
    [Serializable]
    [ActiveRecord("map_coordinates")]
    [AttributeAssociatedFile("MapCoordinates")]
    [D2OClass("MapCoordinates", "com.ankamagames.dofus.datacenter.world")]
    public sealed class MapCoordinateRecord : DataBaseRecord<MapCoordinateRecord>
    {

       [D2OField("compressedCoords")]
       [Property("CompressedCoords")]
       public uint CompressedCoords
       {
           get;
           set;
       }

       [D2OField("mapIds")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public List<int> Id
       {
           get;
           set;
       }

    }
}