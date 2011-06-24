using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Mounts
{
    [Serializable]
    [ActiveRecord("mount_behavior")]
    [AttributeAssociatedFile("MountBehaviors")]
    [D2OClass("MountBehavior", "com.ankamagames.dofus.datacenter.mounts")]
    public sealed class MountBehaviorRecord : DataBaseRecord<MountBehaviorRecord>
    {

       [D2OField("MODULE")]
       [Property("MODULE")]
       public String MODULE
       {
           get;
           set;
       }

       [D2OField("id")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public uint Id
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