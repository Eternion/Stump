using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Npcs
{
    [Serializable]
    [ActiveRecord("tax_collector_name")]
    [AttributeAssociatedFile("TaxCollectorNames")]
    [D2OClass("TaxCollectorName", "com.ankamagames.dofus.datacenter.npcs")]
    public sealed class TaxCollectorNameRecord : DataBaseRecord<TaxCollectorNameRecord>
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

    }
}