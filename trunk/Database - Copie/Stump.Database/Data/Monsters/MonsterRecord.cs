using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.Database.Data.Monters;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Monsters
{
    [Serializable]
    [ActiveRecord("monster")]
    [AttributeAssociatedFile("Monsters")]
    [D2OClass("Monster", "com.ankamagames.dofus.datacenter.monsters")]
    public sealed class MonsterRecord : DataBaseRecord<MonsterRecord>
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

       [D2OField("gfxId")]
       [Property("GfxId")]
       public uint GfxId
       {
           get;
           set;
       }

       [D2OField("race")]
       [Property("Race")]
       public int Race
       {
           get;
           set;
       }

       [D2OField("grades")]
       [Property("Grades", ColumnType="Serializable")]
       public List<MonsterGrade> Grades
       {
           get;
           set;
       }

       [D2OField("look")]
       [Property("Look")]
       public String Look
       {
           get;
           set;
       }

       [D2OField("useSummonSlot")]
       [Property("UseSummonSlot")]
       public Boolean UseSummonSlot
       {
           get;
           set;
       }

       [D2OField("useBombSlot")]
       [Property("UseBombSlot")]
       public Boolean UseBombSlot
       {
           get;
           set;
       }

       [D2OField("canPlay")]
       [Property("CanPlay")]
       public Boolean CanPlay
       {
           get;
           set;
       }

    }
}