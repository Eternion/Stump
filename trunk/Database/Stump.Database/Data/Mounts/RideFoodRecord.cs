using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Mounts
{
    [Serializable]
    [ActiveRecord("ride_food")]
    [AttributeAssociatedFile("RideFood")]
    [D2OClass("RideFood", "com.ankamagames.dofus.datacenter.mounts")]
    public sealed class RideFoodRecord : DataBaseRecord<RideFoodRecord>
    {

       [D2OField("gid")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public uint Id
       {
           get;
           set;
       }

       [D2OField("typeId")]
       [Property("TypeId")]
       public uint TypeId
       {
           get;
           set;
       }

       [D2OField("MODULE")]
       [Property("MODULE")]
       public String MODULE
       {
           get;
           set;
       }

    }
}