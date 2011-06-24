using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Misc
{
    [Serializable]
    [ActiveRecord("title")]
    [AttributeAssociatedFile("Titles")]
    [D2OClass("Title", "com.ankamagames.dofus.datacenter.misc")]
    public sealed class TitleRecord : DataBaseRecord<TitleRecord>
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

       [D2OField("color")]
       [Property("Color")]
       public String Color
       {
           get;
           set;
       }

    }
}