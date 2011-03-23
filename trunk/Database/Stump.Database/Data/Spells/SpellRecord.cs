using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Spells
{
    [Serializable]
    [ActiveRecord("spell")]
    [AttributeAssociatedFile("Spells")]
    [D2OClass("Spell", "com.ankamagames.dofus.datacenter.spells")]
    public sealed class SpellRecord : DataBaseRecord<SpellRecord>
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

       [D2OField("descriptionId")]
       [Property("DescriptionId")]
       public uint DescriptionId
       {
           get;
           set;
       }

       [D2OField("typeId")]
       [Property("TypeId")]
       public uint TypeId
       {
           get;
           set;
       }

       [D2OField("scriptParams")]
       [Property("ScriptParams")]
       public String ScriptParams
       {
           get;
           set;
       }

       [D2OField("scriptParamsCritical")]
       [Property("ScriptParamsCritical")]
       public String ScriptParamsCritical
       {
           get;
           set;
       }

       [D2OField("scriptId")]
       [Property("ScriptId")]
       public int ScriptId
       {
           get;
           set;
       }

       [D2OField("scriptIdCritical")]
       [Property("ScriptIdCritical")]
       public int ScriptIdCritical
       {
           get;
           set;
       }

       [D2OField("iconId")]
       [Property("IconId")]
       public uint IconId
       {
           get;
           set;
       }

       [D2OField("spellLevels")]
       [Property("SpellLevels", ColumnType="Serializable")]
       public List<uint> SpellLevels
       {
           get;
           set;
       }

       [D2OField("useParamCache")]
       [Property("UseParamCache")]
       public Boolean UseParamCache
       {
           get;
           set;
       }

    }
}