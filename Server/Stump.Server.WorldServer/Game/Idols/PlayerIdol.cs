using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Idols;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using System.Collections.Generic;

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

        public double GetSynergy(List<PlayerIdol> idols)
        {
            var coeff = 1d;

            for (var i = 0; i < idols.Count; i++)
            {
                for (var j = 0; j < Template.SynergyIdolsIds.Count; j++)
                {
                    if (Template.SynergyIdolsIds[j] == idols[i].Id)
                    {
                        coeff *= Template.SynergyIdolsCoef[j];
                    }
                }
            }

            return coeff;
        }

        #region Network

        public Idol GetNetworkIdol()
        {
            return new Idol((short)Id, (short)ExperienceBonus, (short)DropBonus);
        }

        public Idol GetNetworkPartyIdol()
        {
            return new PartyIdol((short)Id, (short)ExperienceBonus, (short)DropBonus, new long[] { Owner.Id });
        }

        #endregion Network
    }
}
