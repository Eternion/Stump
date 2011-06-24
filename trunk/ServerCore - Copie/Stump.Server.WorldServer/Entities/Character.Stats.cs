
using System;
using Stump.Server.WorldServer.Threshold;

namespace Stump.Server.WorldServer.Entities
{
    public partial class Character
    {

        public int StatsPoint
        {
            get;
            private set;
        }

        public int SpellsPoints
        {
            get;
            private set;
        }

        public uint Energy
        {
            get;
            private set;
        }

        public uint EnergyMax
        {
            get;
            private set;
        }

        public long Experience
        {
            get;
            private set;
        }

        public long ExperienceMax
        {
            get;
            private set;
        }

        public void GainExperience(long points)
        {
            Experience += points;

            NotifyExperienceGained(points);
            NotifyExperienceModified();
        }

        public void SetExperience(long experience)
        {
            Experience = experience;

            NotifyExperienceModified();
        }

        private void OnExperienceModified(Character character)
        {
            var level = (int)ThresholdManager.CharacterExp.GetLevel(Experience);
            var difference = level - Level;

            if (difference > 0)
            {
                NotifyLeveledUp((uint)difference);

                Level = level;
                ExperienceMax = ThresholdManager.CharacterExp.GetUpperBound(Level);
            }
            else if (level < Level)
            {
                NotifyLeveledDown((uint)(-difference));

                Level = level;
                ExperienceMax = ThresholdManager.CharacterExp.GetUpperBound(Level);
            }
        }
    }
}