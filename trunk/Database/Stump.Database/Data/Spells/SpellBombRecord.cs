using System;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Database.Data.Spells
{
    [Serializable]
    [ActiveRecord("spell_bomb")]
    [AttributeAssociatedFile("SpellBombs")]
    [D2OClass("SpellBomb", "com.ankamagames.dofus.datacenter.spells")]
    public sealed class SpellBombRecord : DataBaseRecord<SpellBombRecord>
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