using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Alignments
{
    [Serializable]
    [ActiveRecord("alignment_side")]
    [AttributeAssociatedFile("AlignmentSides")]
    [D2OClass("AlignmentSide", "com.ankamagames.dofus.datacenter.alignments")]
    public sealed class AlignmentSideRecord : DataBaseRecord<AlignmentSideRecord>
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

       [D2OField("canConquest")]
       [Property("CanConquest")]
       public Boolean CanConquest
       {
           get;
           set;
       }

    }
}