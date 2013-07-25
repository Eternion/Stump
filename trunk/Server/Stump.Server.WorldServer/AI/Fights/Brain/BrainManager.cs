using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Stump.Core.Reflection;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.AI.Fights.Brain
{
    public class BrainManager : Singleton<BrainManager>
    {
        protected static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly Dictionary<int, Type> m_brains = new Dictionary<int, Type>();

        [Initialization(InitializationPass.Fourth)]
        public void Initialize()
        {
            RegisterAll(Assembly.GetExecutingAssembly());
        }

        public void RegisterAll(Assembly assembly)
        {
            if (assembly != null)
            {
                foreach (Type type in assembly.GetTypes().Where( x => x.IsSubclassOf(typeof(Brain))))
                {
                        RegisterBrain(type);
                }
            }
        }

        public void RegisterBrain(Type brain)
        {
            var brainIdentifierAttributes = (brain.GetCustomAttributes(typeof (BrainIdentifierAttribute))) as IEnumerable<BrainIdentifierAttribute>;
            if (brainIdentifierAttributes != null)
            foreach (var brainIdentifierAttribute in brainIdentifierAttributes)
            {
                var identifiers = brainIdentifierAttribute.Identifiers;

                foreach (var identifier in identifiers)
                {
                    if (!m_brains.ContainsKey(identifier))
                    {
                        m_brains.Add(identifier, brain);
                    }
                }
            }
        }

        public Brain GetDefaultBrain(AIFighter fighter)
        {
            return new Brain(fighter);
        }

        public Brain GetBrain(int identifier, AIFighter fighter)
        {
            if (m_brains.ContainsKey(identifier))
            {
                var brainType = m_brains[identifier];
                return (Brain) Activator.CreateInstance(brainType, fighter);
            }
            else
            {
                return GetDefaultBrain(fighter);
            }
        }
    }
}
