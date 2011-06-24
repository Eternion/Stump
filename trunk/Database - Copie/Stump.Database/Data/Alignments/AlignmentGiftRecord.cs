using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Alignments
{
    [Serializable]
    [ActiveRecord("alignment_gift")]
    [AttributeAssociatedFile("AlignmentGift")]
    [D2OClass("AlignmentGift", "com.ankamagames.dofus.datacenter.alignments")]
    public sealed class AlignmentGiftRecord : DataBaseRecord<AlignmentGiftRecord>
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

       [D2OField("effectId")]
       [Property("EffectId")]
       public int EffectId
       {
           get;
           set;
       }

       [D2OField("gfxId")]
       [Property("GfxId")]
       public uint GfxId
       {
           get;
           set;
       }

    }
}