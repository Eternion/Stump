using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Alignments
{
    [Serializable]
    [ActiveRecord("alignment_rank")]
    [AttributeAssociatedFile("AlignmentRank")]
    [D2OClass("AlignmentRank", "com.ankamagames.dofus.datacenter.alignments")]
    public sealed class AlignmentRankRecord : DataBaseRecord<AlignmentRankRecord>
    {

       [D2OField("id")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public int Id
       {
           get;
           set;
       }

       [D2OField("orderId")]
       [Property("OrderId")]
       public uint OrderId
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

       [D2OField("descriptionId")]
       [Property("DescriptionId")]
       public uint DescriptionId
       {
           get;
           set;
       }

       [D2OField("minimumAlignment")]
       [Property("MinimumAlignment")]
       public int MinimumAlignment
       {
           get;
           set;
       }

       [D2OField("objectsStolen")]
       [Property("ObjectsStolen")]
       public int ObjectsStolen
       {
           get;
           set;
       }

       [D2OField("gifts")]
       [Property("Gifts", ColumnType="Serializable")]
       public List<int> Gifts
       {
           get;
           set;
       }

    }
}