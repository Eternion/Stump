using System;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Server.WorldServer.Database.Spells
{
    [Serializable]
    [ActiveRecord("spells_state")]
    [D2OClass("SpellState", "com.ankamagames.dofus.datacenter.spells")]
    public sealed class SpellState : WorldBaseRecord<SpellState>
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

       [D2OField("preventsSpellCast")]
       [Property("PreventsSpellCast")]
       public Boolean PreventsSpellCast
       {
           get;
           set;
       }

       [D2OField("preventsFight")]
       [Property("PreventsFight")]
       public Boolean PreventsFight
       {
           get;
           set;
       }

       [D2OField("critical")]
       [Property("Critical")]
       public Boolean Critical
       {
           get;
           set;
       }

    }
}