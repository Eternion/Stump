using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Misc
{
    [Serializable]
    [ActiveRecord("pack")]
    [AttributeAssociatedFile("Pack")]
    [D2OClass("Pack", "com.ankamagames.dofus.datacenter.misc")]
    public sealed class PackRecord : DataBaseRecord<PackRecord>
    {

       [D2OField("id")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public int Id
       {
           get;
           set;
       }

       [D2OField("name")]
       [Property("Name")]
       public String Name
       {
           get;
           set;
       }

       [D2OField("hasSubAreas")]
       [Property("HasSubAreas")]
       public Boolean HasSubAreas
       {
           get;
           set;
       }

    }
}