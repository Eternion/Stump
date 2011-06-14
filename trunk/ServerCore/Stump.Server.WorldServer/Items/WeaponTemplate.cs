
using WeaponTemplateEx = Stump.DofusProtocol.D2oClasses.Weapon;
using ItemTemplateEx = Stump.DofusProtocol.D2oClasses.Item;

namespace Stump.Server.WorldServer.Items
{
    public class WeaponTemplate : ItemTemplate
    {
        public WeaponTemplate(WeaponTemplateEx basedclass) :
            base(basedclass)
        {
        }

        public int ApCost
        {
            get { return ((WeaponTemplateEx) m_basedclass).apCost; }
        }

        public int MinRange
        {
            get { return ((WeaponTemplateEx) m_basedclass).minRange; }
        }

        public int Range
        {
            get { return ((WeaponTemplateEx) m_basedclass).range; }
        }

        public bool CastInLine
        {
            get { return ((WeaponTemplateEx) m_basedclass).castInLine; }
        }

        public bool CastTestLos
        {
            get { return ((WeaponTemplateEx) m_basedclass).castTestLos; }
        }

        public int CriticalHitProbability
        {
            get { return ((WeaponTemplateEx) m_basedclass).criticalHitProbability; }
        }

        public int CriticalHitBonus
        {
            get { return ((WeaponTemplateEx) m_basedclass).criticalHitBonus; }
        }

        public int CriticalFailureProbability
        {
            get { return ((WeaponTemplateEx) m_basedclass).criticalFailureProbability; }
        }
    }
}