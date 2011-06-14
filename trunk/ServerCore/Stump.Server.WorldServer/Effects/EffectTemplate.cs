
using Stump.DofusProtocol.Enums;
using EffectTemplateEx = Stump.DofusProtocol.D2oClasses.Effect;

namespace Stump.Server.WorldServer.Effects
{
    public class EffectTemplate
    {
        private readonly uint m_category;
        private readonly int m_characteristic;
        private readonly int m_id;
        private readonly string m_operator;

        public EffectTemplate(EffectTemplateEx effect)
        {
            m_id = effect.id;
            m_category = effect.category;
            m_operator = effect.@operator;
            m_characteristic = effect.characteristic;
        }

        public int Id
        {
            get { return m_id; }
        }

        public EffectsEnum EffectId
        {
            get { return (EffectsEnum) Id; }
        }

        public uint Category
        {
            get { return m_category; }
        }

        public int Characteristic
        {
            get { return m_characteristic; }
        }

        public string @Operator
        {
            get { return m_operator; }
        }
    }
}