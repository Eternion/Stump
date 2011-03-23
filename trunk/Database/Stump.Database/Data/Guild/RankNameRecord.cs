using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Guild
{
    [Serializable]
    [ActiveRecord("rank_name")]
    [AttributeAssociatedFile("RankNames")]
    [D2OClass("RankName", "com.ankamagames.dofus.datacenter.guild")]
    public sealed class RankNameRecord : DataBaseRecord<RankNameRecord>
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

       [D2OField("order")]
       [Property("Order")]
       public int Order
       {
           get;
           set;
       }

    }
}