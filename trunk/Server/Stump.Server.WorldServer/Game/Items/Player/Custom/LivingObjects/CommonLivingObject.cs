using System;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;

namespace Stump.Server.WorldServer.Game.Items.Player.Custom.LivingObjects
{
    public abstract class CommonLivingObject : BasePlayerItem
    {
        protected static short[] LevelsSteps =
        {
            0, 10, 21, 33, 46, 60, 75, 91, 108, 126, 145, 165, 186, 208, 231, 255,
            280, 306, 333, 361
        };

        private LivingObjectRecord m_record;
        private EffectInteger m_categoryEffect;
        private EffectInteger m_experienceEffect;
        private EffectInteger m_moodEffect;
        private EffectDate m_lastMealEffect;
        private EffectInteger m_selectedLevelEffect;

        protected CommonLivingObject(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
            
        }
        protected virtual void Initialize()
        {
            if ((m_moodEffect = (EffectInteger)
                (Effects.FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_LivingObjectMood))) == null)
            {
                m_moodEffect = new EffectInteger(EffectsEnum.Effect_LivingObjectMood, 0);
                Effects.Add(m_moodEffect);
            }

            if ((m_selectedLevelEffect = (EffectInteger)
                (Effects.FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_LivingObjectSkin))) == null)
            {
                m_selectedLevelEffect = new EffectInteger(EffectsEnum.Effect_LivingObjectSkin, 1);
                Effects.Add(m_selectedLevelEffect);
            }

            if ((m_experienceEffect = (EffectInteger)
                (Effects.FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_LivingObjectLevel))) == null)
            {
                m_experienceEffect = new EffectInteger(EffectsEnum.Effect_LivingObjectLevel, 0);
                Effects.Add(m_experienceEffect);
            }


            if ((m_categoryEffect = (EffectInteger)
                (Effects.FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_LivingObjectCategory))) != null) return;
            m_categoryEffect = new EffectInteger(EffectsEnum.Effect_LivingObjectCategory, (short) m_record.ItemType);
            Effects.Add(m_categoryEffect);

            OnObjectModified();
        }

        protected LivingObjectRecord LivingObjectRecord
        {
            get { return m_record; }
            set { m_record = value; }
        }

        protected EffectInteger SelectedLevelEffect
        {
            get { return m_selectedLevelEffect; }
            set
            {
                m_selectedLevelEffect = value;
                OnObjectModified();
            }
        }

        protected EffectInteger CategoryEffect
        {
            get { return m_categoryEffect; }
            set
            {
                m_categoryEffect = value;
                OnObjectModified();
            }
        }

        protected EffectInteger ExperienceEffect
        {
            get { return m_experienceEffect; }
            set
            {
                m_experienceEffect = value;
                OnObjectModified();
            }
        }

        protected EffectInteger MoodEffect
        {
            get { return m_moodEffect; }
            set
            {
                m_moodEffect = value;
                OnObjectModified();
            }
        }

        protected EffectDate LastMealEffect
        {
            get { return m_lastMealEffect; }
            set
            {
                m_lastMealEffect = value;
                OnObjectModified();
            }
        }

        
        public short Mood
        {
            get { return m_moodEffect.Value; }
            set
            {
                m_moodEffect.Value = value;
                OnObjectModified();
                Invalidate();
            }
        }

        public short Experience
        {
            get { return m_experienceEffect.Value; }
            set
            {
                m_experienceEffect.Value = value;
                OnObjectModified();
                Invalidate();
            }
        }

        public short Level
        {
            get
            {
                short level = 1;
                while (LevelsSteps.Length > level && LevelsSteps[level] <= Experience)
                    level++;

                return level;
            }
        }

        public short SelectedLevel
        {
            get { return m_selectedLevelEffect.Value; }
            set
            {
                if (value <= 0 || value > Level) return;
                m_selectedLevelEffect.Value = value;

                Invalidate();
                Owner.Inventory.RefreshItem(this);
                Owner.UpdateLook();
                OnObjectModified();
            }
        }

        public int IconId
        {
            get
            {
                var icon = Template.IconId;

                if (m_record.Moods.Count > Mood && m_record.Moods[Mood].Count > SelectedLevel - 1)
                    icon = m_record.Moods[Mood][SelectedLevel - 1];

                return icon;
            }
        }

        public override uint AppearanceId
        {
            get
            {
                var skin = Template.AppearanceId;

                if (SelectedLevel > 0 && m_record.Skins.Count > SelectedLevel - 1)
                    skin = (uint) m_record.Skins[SelectedLevel - 1];

                return skin;
            }
        }

        public DateTime? LastMeal
        {
            get
            {
                return m_lastMealEffect != null ? m_lastMealEffect.GetDate() : (DateTime?) null;
            }
            set
            {
                if (value == null)
                {
                    m_lastMealEffect = null;
                    Effects.RemoveAll(x => x.EffectId == EffectsEnum.Effect_LastMeal);
                }
                else if (m_lastMealEffect == null)
                {
                    m_lastMealEffect = new EffectDate(EffectsEnum.Effect_LastMeal, value.Value);
                    Effects.Add(m_lastMealEffect);
                }

                else
                    m_lastMealEffect.SetDate(value.Value);

                OnObjectModified();
                Invalidate();
            }
        }

    }
}