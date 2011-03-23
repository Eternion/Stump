using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Alignments
{
    [Serializable]
    [ActiveRecord("alignment_balance")]
    [AttributeAssociatedFile("AlignmentBalance")]
    [D2OClass("AlignmentBalance", "com.ankamagames.dofus.datacenter.alignments")]
    public sealed class AlignmentBalanceRecord : DataBaseRecord<AlignmentBalanceRecord>
    {

       [D2OField("id")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public int Id
       {
           get;
           set;
       }

       [D2OField("startValue")]
       [Property("StartValue")]
       public int StartValue
       {
           get;
           set;
       }

       [D2OField("endValue")]
       [Property("EndValue")]
       public int EndValue
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

    }
}