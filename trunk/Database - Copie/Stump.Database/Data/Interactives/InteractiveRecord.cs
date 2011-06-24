using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Interactives
{
    [Serializable]
    [ActiveRecord("interactive")]
    [AttributeAssociatedFile("Interactives")]
    [D2OClass("Interactive", "com.ankamagames.dofus.datacenter.interactives")]
    public sealed class InteractiveRecord : DataBaseRecord<InteractiveRecord>
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

       [D2OField("actionId")]
       [Property("ActionId")]
       public int ActionId
       {
           get;
           set;
       }

    }
}