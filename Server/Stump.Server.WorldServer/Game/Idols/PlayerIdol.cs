using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Idols;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Idols
{
    public class PlayerIdol
    {
        public PlayerIdol(Character owner, IdolTemplate idolTemplate)
        {
            Owner = owner;
            Template = idolTemplate;

        }

        public Character Owner
        {
            get;
            private set;
        }

        public IdolTemplate Template
        {
            get;
            private set;
        }

        public int Id
        {
            get { return Template.Id; }
        }

        public int ExperienceBonus
        {
            get { return Template.ExperienceBonus; }
        }

        public int DropBonus
        {
            get { return Template.DropBonus; }
        }

        public double GetSynergyWith(IdolTemplate template)
        {
            var index = Template.SynergyIdolsIds.FindIndex(x => x == template.Id);
            return Template.SynergyIdolsCoef[index];
        }

        #region Network

        public Idol GetNetworkIdol()
        {
            return new Idol((short)Id, (short)ExperienceBonus, (short)DropBonus);
        }

        #endregion Network
    }
}
