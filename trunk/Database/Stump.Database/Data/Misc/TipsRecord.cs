using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Misc
{
    [Serializable]
    [ActiveRecord("tips")]
    [AttributeAssociatedFile("Tips")]
    [D2OClass("Tips", "com.ankamagames.dofus.datacenter.misc")]
    public sealed class TipRecord : DataBaseRecord<TipRecord>
    {

       [D2OField("id")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public int Id
       {
           get;
           set;
       }

       [D2OField("descId")]
       [Property("DescId")]
       public uint DescId
       {
           get;
           set;
       }

    }
}