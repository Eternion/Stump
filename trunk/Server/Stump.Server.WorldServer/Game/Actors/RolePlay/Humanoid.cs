using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Guilds;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay
{
    public abstract class Humanoid : NamedActor
    {
        private List<RolePlayActor> m_followingCharacters = new List<RolePlayActor>();

        public IEnumerable<RolePlayActor> FollowingCharacters
        {
            get { return m_followingCharacters; }
        }

        public void AddFollowingCharacter(RolePlayActor actor)
        {
            m_followingCharacters.Add(actor);
        }

        public void RemoveFollowingCharacter(RolePlayActor actor)
        {
            m_followingCharacters.Remove(actor);
        }

        public virtual SexTypeEnum Sex
        {
            get;
            protected set;
        }

        #region Network

        #region HumanInformations

        public virtual HumanInformations GetHumanInformations(Character character)
        {
            //IEnumerable<HumanOption> hOptions = new HumanOption[] { new HumanOptionEmote(22, DateTime.Now.GetUnixTimeStamp()), new HumanOptionGuild(new GuildInformations(61001, "Staff", new GuildEmblem(104, 9864735, 15, 16777215))) };
            var hOptions = Enumerable.Empty<HumanOption>();

            var guildId = character.GuildId;

            if (guildId != 0)
            {

                var guildInfo = GuildManager.Instance.FindById(guildId);
                hOptions = new HumanOption[]
                    {
                        new HumanOptionGuild(new GuildInformations(guildId, guildInfo.Name, guildInfo.GuildEmblem))
                    };
            }

            return new HumanInformations(new ActorRestrictionsInformations(),
                Sex == SexTypeEnum.SEX_FEMALE,
                hOptions); // todo
        }

        #endregion 

	    #endregion
    }
}