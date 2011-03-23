using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.AmbientSounds
{
    [Serializable]
    [ActiveRecord("ambient_sound")]
    [AttributeAssociatedFile("AmbientSounds")]
    [D2OClass("AmbientSound", "com.ankamagames.dofus.datacenter.ambientSounds")]
    public sealed class AmbientSoundRecord : DataBaseRecord<AmbientSoundRecord>
    {

       [D2OField("id")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public int Id
       {
           get;
           set;
       }

       [D2OField("volume")]
       [Property("Volume")]
       public uint Volume
       {
           get;
           set;
       }

       [D2OField("criterionId")]
       [Property("CriterionId")]
       public int CriterionId
       {
           get;
           set;
       }

       [D2OField("silenceMin")]
       [Property("SilenceMin")]
       public uint SilenceMin
       {
           get;
           set;
       }

       [D2OField("silenceMax")]
       [Property("SilenceMax")]
       public uint SilenceMax
       {
           get;
           set;
       }

       [D2OField("channel")]
       [Property("Channel")]
       public int Channel
       {
           get;
           set;
       }

    }
}