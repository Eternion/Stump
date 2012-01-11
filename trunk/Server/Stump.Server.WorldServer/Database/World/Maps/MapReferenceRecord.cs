using System;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Server.WorldServer.Database.World.Maps
{
    [Serializable]
    [ActiveRecord("maps_reference")]
    [D2OClass("MapReference", "com.ankamagames.dofus.datacenter.world")]
    public sealed class MapReferenceRecord : WorldBaseRecord<MapReferenceRecord>
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