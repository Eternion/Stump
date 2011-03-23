using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.World
{
    [Serializable]
    [ActiveRecord("map_reference")]
    [AttributeAssociatedFile("MapReferences")]
    [D2OClass("MapReference", "com.ankamagames.dofus.datacenter.world")]
    public sealed class MapReferenceRecord : DataBaseRecord<MapReferenceRecord>
    {

       [D2OField("id")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public int Id
       {
           get;
           set;
       }

       [D2OField("mapId")]
       [Property("MapId")]
       public uint MapId
       {
           get;
           set;
       }

       [D2OField("cellId")]
       [Property("CellId")]
       public int CellId
       {
           get;
           set;
       }

    }
}