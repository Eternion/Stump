using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Stump.Core.Mathematics;
using Stump.Core.Reflection;
using Stump.Server.BaseServer.Initialization;

namespace Stump.Server.WorldServer.Game.Fights.Challenges
{
    public class ChallengeManager : Singleton<ChallengeManager>
    {
        private readonly Dictionary<int, Type> m_challenges = new Dictionary<int, Type>();

        [Initialization(InitializationPass.Fourth)]
        public void Initialize()
        {
            RegisterAll(Assembly.GetExecutingAssembly());
        }

        public void RegisterAll(Assembly assembly)
        {
            if (assembly == null)
                return;

            foreach (var type in assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(DefaultChallenge))))
            {
                RegisterChallenge(type);
            }
        }

        public void RegisterChallenge(Type challenge)
        {
            var challengeIdentifierAttributes = (challenge.GetCustomAttributes(typeof(ChallengeIdentifierAttribute))) as IEnumerable<ChallengeIdentifierAttribute>;
            if (challengeIdentifierAttributes == null)
                return;

            foreach (var identifier in from challengeIdentifierAttribute in challengeIdentifierAttributes
                                       select challengeIdentifierAttribute.Identifiers into identifiers from identifier in identifiers
                                       where !m_challenges.ContainsKey(identifier) select identifier)
            {
                m_challenges.Add(identifier, challenge);
            }
        }

        public DefaultChallenge GetDefaultChallenge(IFight fight)
        {
            return new DefaultChallenge(0, fight);
        }

        public DefaultChallenge GetChallenge(int identifier, IFight fight)
        {
            if (!m_challenges.ContainsKey(identifier))
                return null;

            var challengeType = m_challenges[identifier];
            return (DefaultChallenge)Activator.CreateInstance(challengeType, identifier, fight);
        }

        public DefaultChallenge GetRandomChallenge(IFight fight)
        {
            while (true)
            {
                var random = new CryptoRandom().Next(m_challenges.Keys.Min(), (m_challenges.Keys.Max() + 1));
                var challenge = GetChallenge(random, fight);

                if (challenge == null)
                    continue;

                if (!challenge.IsEligible())
                    continue;

                return challenge;
            }
        }
    }
}
