using System;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Server.WorldServer.Database.Spells
{
    [Serializable]
    [ActiveRecord("spells_bomb")]
    [D2OClass("SpellBomb", "com.ankamagames.dofus.datacenter.spells")]
    public sealed class SpellBombTemplate : WorldBaseRecord<SpellBombTemplate>
    {

       [D2OField("id")]
       [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
       public int Id
       {
           get;
           set;
       }

       [D2OField("chainReactionSpellId")]
       [Property("ChainReactionSpellId")]
       public int ChainReactionSpellId
       {
           get;
           set;
       }

       [D2OField("explodSpellId")]
       [Property("ExplodSpellId")]
       public int ExplodSpellId
       {
           get;
           set;
       }

       [D2OField("wallId")]
       [Property("WallId")]
       public int WallId
       {
           get;
           set;
       }

       [D2OField("instantSpellId")]
       [Property("InstantSpellId")]
       public int InstantSpellId
       {
           get;
           set;
       }

       [D2OField("comboCoeff")]
       [Property("ComboCoeff")]
       public int ComboCoeff
       {
           get;
           set;
       }

    }
}