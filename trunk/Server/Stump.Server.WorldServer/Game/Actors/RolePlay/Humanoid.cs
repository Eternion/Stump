using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Types;

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

        #region Network

        public override GameContextActorInformations GetGameContextActorInformations()
        {
            return new GameRolePlayHumanoidInformations(
                Id,
                Look,
                GetEntityDispositionInformations(),
                Name,
                GetHumanInformations());
        }

        #region HumanInformations

        public virtual HumanInformations GetHumanInformations()
        {
            return new HumanInformations(FollowingCharacters.Select(entry => entry.Look),
                -1, // todo : emote
                0,
                new ActorRestrictionsInformations(), // todo : restrictions
                0, // todo : title
                "");
        }

        #endregion 

	    #endregion
    }
}