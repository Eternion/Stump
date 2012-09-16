using System;
using System.Data.Entity.ModelConfiguration;
using Stump.DofusProtocol.D2oClasses;
using Stump.Server.WorldServer.Database.I18n;

namespace Stump.Server.WorldServer.Database
{
    public class SpellStateConfiguration : EntityTypeConfiguration<SpellState>
    {
        public SpellStateConfiguration()
        {
            ToTable("spells_state");
        }
    }

    [D2OClass("SpellState", "com.ankamagames.dofus.datacenter.spells")]
    public sealed class SpellState : IAssignedByD2O
    {
        private string m_name;

        public int Id
        {
            get;
            set;
        }

        public uint NameId
        {
            get;
            set;
        }

        public string Name
        {
            get { return m_name ?? (m_name = TextManager.Instance.GetText(NameId)); }
        }

        public Boolean PreventsSpellCast
        {
            get;
            set;
        }

        public Boolean PreventsFight
        {
            get;
            set;
        }

        public Boolean Critical
        {
            get;
            set;
        }

        #region IAssignedByD2O Members

        public void AssignFields(object d2oObject)
        {
            var state = (DofusProtocol.D2oClasses.SpellState) d2oObject;
            Id = state.id;
            NameId = state.nameId;
            PreventsSpellCast = state.preventsSpellCast;
            PreventsFight = state.preventsFight;
            Critical = state.critical;
        }

        #endregion
    }
}