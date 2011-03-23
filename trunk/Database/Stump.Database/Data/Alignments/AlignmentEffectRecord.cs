using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Alignments
{
    [Serializable]
    [ActiveRecord("alignment_effect")]
    [AttributeAssociatedFile("AlignmentEffect")]
    [D2OClass("AlignmentEffect", "com.ankamagames.dofus.datacenter.alignments")]
    public sealed class AlignmentEffectRecord : DataBaseRecord<AlignmentEffectRecord>
    {

       [D2OField("id")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public int Id
       {
           get;
           set;
       }

       [D2OField("characteristicId")]
       [Property("CharacteristicId")]
       public uint CharacteristicId
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