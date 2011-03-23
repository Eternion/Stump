using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Items
{
    [Serializable]
    [ActiveRecord("item_type")]
    [AttributeAssociatedFile("ItemTypes")]
    [D2OClass("ItemType", "com.ankamagames.dofus.datacenter.items")]
    public sealed class ItemTypeRecord : DataBaseRecord<ItemTypeRecord>
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

       [D2OField("superTypeId")]
       [Property("SuperTypeId")]
       public uint SuperTypeId
       {
           get;
           set;
       }

       [D2OField("plural")]
       [Property("Plural")]
       public Boolean Plural
       {
           get;
           set;
       }

       [D2OField("gender")]
       [Property("Gender")]
       public uint Gender
       {
           get;
           set;
       }

       [D2OField("zoneSize")]
       [Property("ZoneSize")]
       public uint ZoneSize
       {
           get;
           set;
       }

       [D2OField("zoneShape")]
       [Property("ZoneShape")]
       public uint ZoneShape
       {
           get;
           set;
       }

       [D2OField("needUseConfirm")]
       [Property("NeedUseConfirm")]
       public Boolean NeedUseConfirm
       {
           get;
           set;
       }

    }
}