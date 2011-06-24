using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Mounts
{
    [Serializable]
    [ActiveRecord("mount")]
    [AttributeAssociatedFile("Mounts")]
    [D2OClass("Mount", "com.ankamagames.dofus.datacenter.mounts")]
    public sealed class MountRecord : DataBaseRecord<MountRecord>
    {

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

       [D2OField("look")]
       [Property("Look")]
       public String Look
       {
           get;
           set;
       }

       [D2OField("MODULE")]
       [Property("MODULE")]
       public String MODULE
       {
           get;
           set;
       }

    }
}