using System;
using System.Data.Entity.ModelConfiguration;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;

namespace Stump.Server.WorldServer.Database
{
    public class SpellBombTemplateConfiguration : EntityTypeConfiguration<SpellBombTemplate>
    {
        public SpellBombTemplateConfiguration()
        {
            ToTable("spells_bombs");
        }
    }
    [D2OClass("SpellBomb", "com.ankamagames.dofus.datacenter.spells")]
    public sealed class SpellBombTemplate : IAssignedByD2O
    {

       public int Id
       {
           get;
           set;
       }

       public int ChainReactionSpellId
       {
           get;
           set;
       }

       public int ExplodSpellId
       {
           get;
           set;
       }

       public int WallId
       {
           get;
           set;
       }

       public int InstantSpellId
       {
           get;
           set;
       }

       public int ComboCoeff
       {
           get;
           set;
       }

        public void AssignFields(object d2oObject)
        {
            var spell = (DofusProtocol.D2oClasses.SpellBomb)d2oObject;
            Id = spell.id;
            ChainReactionSpellId = spell.chainReactionSpellId;
            ExplodSpellId = spell.explodSpellId;
            WallId = spell.wallId;
            InstantSpellId = spell.instantSpellId;
            ComboCoeff = spell.comboCoeff;
        }
    }
}