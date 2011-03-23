using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Monsters
{
    [Serializable]
    [ActiveRecord("monster_race")]
    [AttributeAssociatedFile("MonsterRaces")]
    [D2OClass("MonsterRace", "com.ankamagames.dofus.datacenter.monsters")]
    public sealed class MonsterRaceRecord : DataBaseRecord<MonsterRaceRecord>
    {

       [D2OField("id")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public int Id
       {
           get;
           set;
       }

       [D2OField("superRaceId")]
       [Property("SuperRaceId")]
       public int SuperRaceId
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