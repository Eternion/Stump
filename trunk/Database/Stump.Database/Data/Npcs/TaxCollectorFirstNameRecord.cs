using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Npcs
{
    [Serializable]
    [ActiveRecord("tax_collector_firstname")]
    [AttributeAssociatedFile("TaxCollectorFirstnames")]
    [D2OClass("TaxCollectorFirstname", "com.ankamagames.dofus.datacenter.npcs")]
    public sealed class TaxCollectorFirstnameRecord : DataBaseRecord<TaxCollectorFirstnameRecord>
    {

       [D2OField("id")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public int Id
       {
           get;
           set;
       }

       [D2OField("firstnameId")]
       [Property("FirstnameId")]
       public uint FirstnameId
       {
           get;
           set;
       }

    }
}