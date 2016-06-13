using System.Collections.Generic;
using System.Linq;
using Stump.Core.Reflection;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Game.Exchanges.Craft
{
    public class RuneManager : Singleton<RuneManager>
    {
        private Dictionary<EffectsEnum, RuneInformation[]> m_runes = new Dictionary<EffectsEnum, RuneInformation[]>();

        [Initialization(typeof (ItemManager))]
        public void Initiliaze()
        {
            var runes = new Dictionary<EffectsEnum, List<RuneInformation>>();

            foreach (var item in ItemManager.Instance.GetTemplates().Where(x => x.TypeId == (int) ItemTypeEnum.RUNE_DE_FORGEMAGIE))
            {
                foreach (var effect in item.Effects.OfType<EffectDice>())
                {
                    var rune = new RuneInformation(item, effect.Max);

                    if (runes.ContainsKey(effect.EffectId))
                        runes[effect.EffectId].Add(rune);
                    else
                        runes.Add(effect.EffectId, new List<RuneInformation>(new[] {rune}));
                }
            }

            m_runes = runes.ToDictionary(x => x.Key, x => x.Value.ToArray());
        }

        public RuneInformation[] GetEffectRunes(EffectsEnum effect)
        {
            RuneInformation[] runes;
            return m_runes.TryGetValue(effect, out runes) ? runes : new RuneInformation[0];
        }
    }
}