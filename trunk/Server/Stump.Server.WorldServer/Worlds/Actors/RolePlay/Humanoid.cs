using System.Collections.Generic;
using System.Linq;
using Stump.Core.Cache;
using Stump.DofusProtocol.Types;

namespace Stump.Server.WorldServer.Worlds.Actors.RolePlay
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

            m_humanInformations.Invalidate();
        }

        public void RemoveFollowingCharacter(RolePlayActor actor)
        {
            m_followingCharacters.Remove(actor);

            m_humanInformations.Invalidate();
        }

        #region Network
		protected override void InitializeValidators()
        {
            base.InitializeValidators();

            m_humanInformations = new ObjectValidator<HumanInformations>(BuildHumanInformations);
            m_humanInformations.ObjectInvalidated += OnHumanInformationsInvalidation;
        }

        protected override GameContextActorInformations BuildGameContextActorInformations()
        {
            return new GameRolePlayHumanoidInformations(
                Id,
                Look,
                GetEntityDispositionInformations(),
                Name,
                GetHumanInformations());
        }

        #region HumanInformations

        protected ObjectValidator<HumanInformations> m_humanInformations;

        protected virtual void OnHumanInformationsInvalidation(ObjectValidator<HumanInformations> validator)
        {
            m_gameContextActorInformations.Invalidate();
        }

        protected virtual HumanInformations BuildHumanInformations()
        {
            return new HumanInformations(FollowingCharacters.Select(entry => entry.Look),
                0, // todo : emote
                0,
                new ActorRestrictionsInformations(), // todo : restrictions
                0, // todo : title
                "");
        }

        public HumanInformations GetHumanInformations()
        {
            return m_humanInformations;
        }

        #endregion 

	    #endregion
    }
}