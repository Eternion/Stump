using Stump.DofusProtocol.D2oClasses;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace Stump.Server.WorldServer.Database
{
    public class SpellBombRelator
    {
        public static string FetchQuery = "SELECT * FROM spells_bombs";
    }

    [TableName("spells_bombs")]
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

        #region IAssignedByD2O Members

        public void AssignFields(object d2oObject)
        {
            var spell = (SpellBomb) d2oObject;
            Id = spell.id;
            ChainReactionSpellId = spell.chainReactionSpellId;
            ExplodSpellId = spell.explodSpellId;
            WallId = spell.wallId;
            InstantSpellId = spell.instantSpellId;
            ComboCoeff = spell.comboCoeff;
        }

        #endregion
    }
}