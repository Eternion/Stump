using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Monsters
{
    [Serializable]
    [ActiveRecord("monster_super_race")]
    [AttributeAssociatedFile("MonsterSuperRaces")]
    [D2OClass("MonsterSuperRace", "com.ankamagames.dofus.datacenter.monsters")]
    public sealed class MonsterSuperRaceRecord : DataBaseRecord<MonsterSuperRaceRecord>
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