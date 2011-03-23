using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Classes.ambientSounds;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.World
{
    [Serializable]
    [ActiveRecord("map_position")]
    [AttributeAssociatedFile("MapPositions")]
    [D2OClass("MapPosition", "com.ankamagames.dofus.datacenter.world")]
    public sealed class MapPositionRecord : DataBaseRecord<MapPositionRecord>
    {

       [D2OField("id")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public int Id
       {
           get;
           set;
       }

       [D2OField("posX")]
       [Property("PosX")]
       public int PosX
       {
           get;
           set;
       }

       [D2OField("posY")]
       [Property("PosY")]
       public int PosY
       {
           get;
           set;
       }

       [D2OField("outdoor")]
       [Property("Outdoor")]
       public Boolean Outdoor
       {
           get;
           set;
       }

       [D2OField("subAreaId")]
       [Property("SubAreaId")]
       public int SubAreaId
       {
           get;
           set;
       }

       [D2OField("capabilities")]
       [Property("Capabilities")]
       public int Capabilities
       {
           get;
           set;
       }

       [D2OField("worldMap")]
       [Property("WorldMap")]
       public int WorldMap
       {
           get;
           set;
       }

       [D2OField("sounds")]
       [Property("Sounds", ColumnType="Serializable")]
       public List<AmbientSound> Sounds
       {
           get;
           set;
       }

       [D2OField("nameId")]
       [Property("NameId")]
       public int NameId
       {
           get;
           set;
       }

    }
}