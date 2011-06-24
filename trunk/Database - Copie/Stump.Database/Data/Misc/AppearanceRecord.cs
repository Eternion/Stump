using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Misc
{
    [Serializable]
    [ActiveRecord("appearance")]
    [AttributeAssociatedFile("Appearances")]
    [D2OClass("Appearance", "com.ankamagames.dofus.datacenter.misc")]
    public sealed class AppearanceRecord : DataBaseRecord<AppearanceRecord>
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

       [D2OField("type")]
       [Property("Type")]
       public uint Type
       {
           get;
           set;
       }

       [D2OField("data")]
       [Property("Data")]
       public String Data
       {
           get;
           set;
       }

    }
}