using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NLog;
using Stump.Core.Reflection;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Database.AI;
using Stump.Server.BaseServer.Database;

namespace Stump.Server.WorldServer.AI.Fights.Brain
{
    public class BrainManager : Singleton<BrainManager>
    {
        protected static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly Dictionary<int, Type> m_brains = new Dictionary<int, Type>();
        private Dictionary<int, InitBrainRecord> m_initBrains = new Dictionary<int, InitBrainRecord>();

        [Initialization(InitializationPass.Fourth)]
        public void Initialize()
        {
            RegisterAll(Assembly.GetExecutingAssembly());
            m_initBrains = DataManager.DefaultDatabase.Query<InitBrainRecord>(InitBrainRelator.FetchQuery).ToDictionary(entry => entry.MonsterId);
        }

        public void RegisterAll(Assembly assembly)
        {
            if (assembly == null)
                return;

            foreach (var type in assembly.GetTypes().Where( x => x.IsSubclassOf(typeof(Brain))))
            {
                RegisterBrain(type);
            }
        }

        public void RegisterBrain(Type brain)
        {
            var brainIdentifierAttributes = (brain.GetCustomAttributes(typeof (BrainIdentifierAttribute))) as IEnumerable<BrainIdentifierAttribute>;
            if (brainIdentifierAttributes == null)
                return;

            foreach (var identifier in from brainIdentifierAttribute in brainIdentifierAttributes select brainIdentifierAttribute.Identifiers into identifiers
                                       from identifier in identifiers where !m_brains.ContainsKey(identifier) select identifier)
            {
                m_brains.Add(identifier, brain);
            }
        }

        public Brain GetDefaultBrain(AIFighter fighter)
        {
            return new Brain(fighter);
        }

        public Brain GetBrain(int identifier, AIFighter fighter)
        {
            if (m_initBrains.ContainsKey(identifier))
            {
                return new InitBrain(fighter, m_initBrains[identifier]);
            }

            if (!m_brains.ContainsKey(identifier))
            {
                return GetDefaultBrain(fighter);
            }

            return (Brain) Activator.CreateInstance(m_brains[identifier], fighter);
        }
    }
}
