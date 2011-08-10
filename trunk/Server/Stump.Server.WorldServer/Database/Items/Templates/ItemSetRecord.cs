using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Server.WorldServer.Database.Items.Templates
{
    [Serializable]
    [ActiveRecord("items_set")]
    [D2OClass("ItemSet", "com.ankamagames.dofus.datacenter.items")]
    public sealed class ItemSetRecord : WorldBaseRecord<ItemSetRecord>
    {

       [D2OField("id")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public uint Id
       {
           get;
           set;
       }

       [D2OField("items")]
       [Property("Items", ColumnType="Serializable")]
       public List<uint> Items
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

       [D2OField("bonusIsSecret")]
       [Property("BonusIsSecret")]
       public Boolean BonusIsSecret
       {
           get;
           set;
       }

    }
}