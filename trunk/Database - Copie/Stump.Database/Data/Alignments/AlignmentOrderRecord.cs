using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Alignments
{
    [Serializable]
    [ActiveRecord("alignment_order")]
    [AttributeAssociatedFile("AlignmentOrder")]
    [D2OClass("AlignmentOrder", "com.ankamagames.dofus.datacenter.alignments")]
    public sealed class AlignmentOrderRecord : DataBaseRecord<AlignmentOrderRecord>
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

       [D2OField("sideId")]
       [Property("SideId")]
       public uint SideId
       {
           get;
           set;
       }

    }
}