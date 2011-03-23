using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Abuse
{
    [Serializable]
    [ActiveRecord("abuse_reasons")]
    [AttributeAssociatedFile("AbuseReasons")]
    [D2OClass("AbuseReasons", "com.ankamagames.dofus.datacenter.abuse")]
    public sealed class AbuseReasonRecord : DataBaseRecord<AbuseReasonRecord>
    {

       [D2OField("_abuseReasonId")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public uint Id
       {
           get;
           set;
       }

       [D2OField("_mask")]
       [Property("Mask")]
       public uint Mask
       {
           get;
           set;
       }

       [D2OField("_reasonTextId")]
       [Property("ReasonTextId")]
       public int ReasonTextId
       {
           get;
           set;
       }

    }
}