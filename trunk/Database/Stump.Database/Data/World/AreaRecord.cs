using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.World
{
    [Serializable]
    [ActiveRecord("area")]
    [AttributeAssociatedFile("Areas")]
    [D2OClass("Area", "com.ankamagames.dofus.datacenter.world")]
    public sealed class AreaRecord : DataBaseRecord<AreaRecord>
    {

       [D2OField("id")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public int Id
       {
           get;
           set;
       }

       [D2OField("nameId")]
       [Property("NameId")]
       public uint NameId
       {
           get;
           set;
       }

       [D2OField("superAreaId")]
       [Property("SuperAreaId")]
       public int SuperAreaId
       {
           get;
           set;
       }

       [D2OField("containHouses")]
       [Property("ContainHouses")]
       public Boolean ContainHouses
       {
           get;
           set;
       }

       [D2OField("containPaddocks")]
       [Property("ContainPaddocks")]
       public Boolean ContainPaddocks
       {
           get;
           set;
       }

       [D2OField("bounds")]
       [Property("Bounds", ColumnType="Serializable")]
       public Rectangle Bounds
       {
           get;
           set;
       }

    }
}