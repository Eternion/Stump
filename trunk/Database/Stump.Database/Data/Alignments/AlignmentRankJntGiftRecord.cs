using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Alignments
{
    [Serializable]
    [ActiveRecord("alignment_rank_jnt_gift")]
    [AttributeAssociatedFile("AlignmentRankJntGift")]
    [D2OClass("AlignmentRankJntGift", "com.ankamagames.dofus.datacenter.alignments")]
    public sealed class AlignmentRankJntGiftRecord : DataBaseRecord<AlignmentRankJntGiftRecord>
    {

       [D2OField("id")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public int Id
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

       [D2OField("parameters")]
       [Property("Parameters", ColumnType="Serializable")]
       public List<int> Parameters
       {
           get;
           set;
       }

       [D2OField("levels")]
       [Property("Levels", ColumnType="Serializable")]
       public List<int> Levels
       {
           get;
           set;
       }

    }
}