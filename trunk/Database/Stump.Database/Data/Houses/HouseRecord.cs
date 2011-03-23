using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Houses
{
    [Serializable]
    [ActiveRecord("house")]
    [AttributeAssociatedFile("Houses")]
    [D2OClass("House", "com.ankamagames.dofus.datacenter.houses")]
    public sealed class HouseRecord : DataBaseRecord<HouseRecord>
    {

       [D2OField("typeId")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public int Id
       {
           get;
           set;
       }

       [D2OField("defaultPrice")]
       [Property("DefaultPrice")]
       public uint DefaultPrice
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

       [D2OField("descriptionId")]
       [Property("DescriptionId")]
       public int DescriptionId
       {
           get;
           set;
       }

       [D2OField("gfxId")]
       [Property("GfxId")]
       public int GfxId
       {
           get;
           set;
       }

    }
}