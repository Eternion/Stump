using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Alignments
{
    [Serializable]
    [ActiveRecord("alignment_title")]
    [AttributeAssociatedFile("AlignmentTitles")]
    [D2OClass("AlignmentTitle", "com.ankamagames.dofus.datacenter.alignments")]
    public sealed class AlignmentTitleRecord : DataBaseRecord<AlignmentTitleRecord>
    {

       [D2OField("sideId")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public int Id
       {
           get;
           set;
       }

       [D2OField("namesId")]
       [Property("NamesId", ColumnType="Serializable")]
       public List<int> NamesId
       {
           get;
           set;
       }

       [D2OField("shortsId")]
       [Property("ShortsId", ColumnType="Serializable")]
       public List<int> ShortsId
       {
           get;
           set;
       }

    }
}