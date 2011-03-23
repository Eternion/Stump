using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Spells
{
    [Serializable]
    [ActiveRecord("spell_type")]
    [AttributeAssociatedFile("SpellTypes")]
    [D2OClass("SpellType", "com.ankamagames.dofus.datacenter.spells")]
    public sealed class SpellTypeRecord : DataBaseRecord<SpellTypeRecord>
    {

       [D2OField("id")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public int Id
       {
           get;
           set;
       }

       [D2OField("longNameId")]
       [Property("LongNameId")]
       public uint LongNameId
       {
           get;
           set;
       }

       [D2OField("shortNameId")]
       [Property("ShortNameId")]
       public uint ShortNameId
       {
           get;
           set;
       }

    }
}