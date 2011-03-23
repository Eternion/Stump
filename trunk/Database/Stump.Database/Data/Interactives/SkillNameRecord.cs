using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Interactives
{
    [Serializable]
    [ActiveRecord("skill_name")]
    [AttributeAssociatedFile("SkillNames")]
    [D2OClass("SkillName", "com.ankamagames.dofus.datacenter.interactives")]
    public sealed class SkillNameRecord : DataBaseRecord<SkillNameRecord>
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