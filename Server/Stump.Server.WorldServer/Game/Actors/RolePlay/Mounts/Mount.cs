using System.Collections.Generic;
using System.Linq;
using NLog;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.Mounts;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Handlers.Basic;
using Stump.Server.WorldServer.Handlers.Mounts;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.Mounts
{
    public class Mount
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private static readonly double[][] XP_PER_GAP =
        {
            new double[] {0, 10},
            new double[] {10, 8},
            new double[] {20, 6},
            new double[] {30, 4},
            new double[] {40, 3},
            new double[] {50, 2},
            new double[] {60, 1.5},
            new double[] {70, 1}
        };

        [Variable(true)]
        public static int RequiredLevel = 60;

        public Mount(Character character)
        {
            Record = MountManager.Instance.TryGetMountByCharacterId(character.Id);
            Level = ExperienceManager.Instance.GetMountLevel(Experience);
            ExperienceLevelFloor = ExperienceManager.Instance.GetMountLevelExperience(Level);
            ExperienceNextLevelFloor = ExperienceManager.Instance.GetMountNextLevelExperience(Level);
            Effects = MountManager.Instance.GetMountEffects(this);

            Owner = character;

            ApplyMountEffects(false);
        }

        public Mount(MountRecord record)
        {
            Record = record;
            Level = ExperienceManager.Instance.GetMountLevel(Experience);
            ExperienceLevelFloor = ExperienceManager.Instance.GetMountLevelExperience(Level);
            ExperienceNextLevelFloor = ExperienceManager.Instance.GetMountNextLevelExperience(Level);
            Effects = MountManager.Instance.GetMountEffects(this);
        }

        public Mount(bool sex, int templateId)
        {
            Record = new MountRecord
            {
                IsNew = true,
                TemplateId = templateId,
                Experience = ExperienceManager.Instance.GetMountLevelExperience(1)
            };
            Level = ExperienceManager.Instance.GetMountLevel(Experience);
            Sex = sex;
            Name = Model.Name;
            Effects = MountManager.Instance.GetMountEffects(this);
            Behaviors = new List<MountBehaviorEnum>();
        }

        #region Properties

        public MountRecord Record
        {
            get;
            set;
        }

        public bool IsDirty
        {
            get;
            set;
        }

        public int Id
        {
            get { return Record.Id; }
            private set { Record.Id = value; }
        }

        public Character Owner
        {
            get;
            set;
        }

        private int m_ownerId;
        public int OwnerId
        {
            get { return Owner != null ? Owner.Id : m_ownerId; }
            set { m_ownerId = value; }
        }

        public bool IsRiding
        {
            get;
            private set;
        }

        public bool Sex
        {
            get { return Record.Sex; }
            private set
            {
                Record.Sex = value;
                IsDirty = true;
            }

        }

        public List<EffectInteger> Effects
        {
            get;
            private set;
        }

        public List<MountBehaviorEnum> Behaviors
        {
            get { return Record.Behaviors.Select(x => (MountBehaviorEnum)x).ToList(); }
            private set { Record.Behaviors = value.Select(x => (uint)x).ToList(); }
        }

        public MountTemplate Model
        {
            get { return Record.Model; }
        }

        public int TemplateId
        {
            get { return Record.TemplateId; }
            set
            {
                Record.TemplateId = value;
                IsDirty = true;
            }
        }

        public ItemTemplate ScrollItem
        {
            get
            {
                return ItemManager.Instance.TryGetTemplate(Model.ScrollId == 0 ? 7806 : (int)Model.ScrollId);
            }
        }

        public byte Level
        {
            get;
            protected set;
        }

        public long Experience
        {
            get { return Record.Experience; }
            protected set
            {
                Record.Experience = value;
                IsDirty = true;
            }
        }

        public long ExperienceLevelFloor
        {
            get;
            protected set;
        }

        public long ExperienceNextLevelFloor
        {
            get;
            protected set;
        }

        public sbyte GivenExperience
        {
            get { return Record.GivenExperience; }
            protected set
            {
                Record.GivenExperience = value;
                IsDirty = true;
            }
        }

        public string Name
        {
            get { return Record.Name; }
            private set
            {
                Record.Name = value;
                IsDirty = true;
            }
        }

        public int Stamina
        {
            get { return Record.Stamina; }
            protected set
            {
                Record.Stamina = value;
                IsDirty = true;
            }
        }

        public int StaminaMax
        {
            get { return 10000; }
        }

        public int Maturity
        {
            get { return Record.Maturity; }
            protected set
            {
                Record.Maturity = value;
                IsDirty = true;
            }
        }

        public int MaturityForAdult
        {
            get { return 10000; }
        }

        public int Energy
        {
            get { return Record.Energy; }
            protected set
            {
                Record.Energy = value;
                IsDirty = true;
            }
        }

        public int EnergyMax
        {
            get { return 7400; }
        }

        public int Serenity
        {
            get { return Record.Serenity; }
            protected set
            {
                Record.Serenity = value;
                IsDirty = true;
            }
        }

        public int SerenityMax
        {
            get { return 10000; }
        }

        public int AggressivityMax
        {
            get { return -10000; }
        }

        public int Love
        {
            get { return Record.Love; }
            protected set
            {
                Record.Love = value;
                IsDirty = true;
            }
        }

        public int LoveMax
        {
            get { return 10000; }
        }

        public int ReproductionCount
        {
            get { return Record.ReproductionCount; }
            protected set
            {
                Record.ReproductionCount = value;
                IsDirty = true;
            }
        }

        public int ReproductionCountMax
        {
            get { return 80; }
        }

        public int PodsMax
        {
            get { return Record.Model.PodsBase + (Record.Model.PodsPerLevel * Level); }
        }

        public int FecondationTime
        {
            get { return 0; }
        }

        #endregion

        public void ApplyMountEffects(bool send = true)
        {
            if (Owner == null)
                return;

            var item = ItemManager.Instance.CreatePlayerItem(Owner, 7806, 1);
            item.Effects.AddRange(Effects);

            Owner.Inventory.ApplyItemEffects(item, send, true);
        }

        public void UnApplyMountEffects()
        {
            if (Owner == null)
                return;

            var item = ItemManager.Instance.CreatePlayerItem(Owner, 7806, 1);
            item.Effects.AddRange(Effects);

            Owner.Inventory.ApplyItemEffects(item);
        }

        public void RenameMount(Character character, string name)
        {
            Name = name;

            MountHandler.SendMountRenamedMessage(character.Client, Id, name);
        }

        public void Release(Character character)
        {
            Dismount(character);

            UnApplyMountEffects();

            MountHandler.SendMountUnSetMessage(character.Client);
            MountHandler.SendMountReleaseMessage(character.Client, character.Mount.Id);

            MountManager.Instance.UnlinkMountFromCharacter(character);
            character.Mount = null;
        }

        public void Sterelize(Character character)
        {
            character.Mount.ReproductionCount = -1;
            MountHandler.SendMountSterelizeMessage(character.Client, character.Mount.Id);
        }

        public void SetGivenExperience(Character character, sbyte xp)
        {
            GivenExperience = xp > 90 ? (sbyte)90 : (xp < 0 ? (sbyte)0 : xp);

            MountHandler.SendMountXpRatioMessage(character.Client, GivenExperience);
        }

        public void ToggleRiding(Character character)
        {
            if (character.IsBusy() || character.IsInFight())
            {
                //Une action est déjà en cours. Impossible de monter ou de descendre de votre monture.
                BasicHandler.SendTextInformationMessage(character.Client, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 355);
                return;
            }

            IsRiding = !IsRiding;

            character.RefreshActor();

            MountHandler.SendMountRidingMessage(character.Client, IsRiding);

            if (!IsRiding)
            {
                //Vous descendez de votre monture.
                BasicHandler.SendTextInformationMessage(character.Client, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 273);
            }
        }

        public void Dismount(Character character)
        {
            IsRiding = false;

            character.RefreshActor();

            MountHandler.SendMountRidingMessage(character.Client, false);
        }

        public void AddXP(Character character, long experience)
        {
            Experience += experience;

            var level = ExperienceManager.Instance.GetMountLevel(Experience);

            if (level == Level)
                return;

            Level = level;
            OnLevelChanged(character);
        }

        public void AddBehavior(MountBehaviorEnum behavior)
        {
            var behaviors = Behaviors;
            behaviors.Add(behavior);

            Behaviors = behaviors;
        }

        protected virtual void OnLevelChanged(Character character)
        {
            ExperienceLevelFloor = ExperienceManager.Instance.GetMountLevelExperience(Level);
            ExperienceNextLevelFloor = ExperienceManager.Instance.GetMountNextLevelExperience(Level);

            UnApplyMountEffects();
            Effects = MountManager.Instance.GetMountEffects(this);
            ApplyMountEffects();

            MountHandler.SendMountSetMessage(character.Client, GetMountClientData());
        }

        public long AdjustGivenExperience(Character giver, long amount)
        {
            var gap = giver.Level - Level;

            for (var i = XP_PER_GAP.Length - 1; i >= 0; i--)
            {
                if (gap > XP_PER_GAP[i][0])
                    return (long)(amount * XP_PER_GAP[i][1] * 0.01);
            }

            return (long)(amount * XP_PER_GAP[0][1] * 0.01);
        }

        #region Network

        public MountClientData GetMountClientData()
        {
            return new MountClientData
            {
                sex = Sex,
                isRideable = true,
                isWild = false,
                isFecondationReady = false,
                id = Id,
                model = Model.Id,
                ancestor = new int[0],
                behaviors = Behaviors.Select(x => (int)x),
                name = Name,
                ownerId = OwnerId,
                experience = Experience,
                experienceForLevel = ExperienceLevelFloor,
                experienceForNextLevel = ExperienceNextLevelFloor,
                level = (sbyte)Level,
                maxPods = PodsMax,
                stamina = Stamina,
                staminaMax = StaminaMax,
                maturity = Maturity,
                maturityForAdult = MaturityForAdult,
                energy = Energy,
                energyMax = EnergyMax,
                serenity = Serenity,
                serenityMax = SerenityMax,
                aggressivityMax = AggressivityMax,
                love = Love,
                loveMax = LoveMax,
                fecondationTime = FecondationTime,
                boostLimiter = 100,
                boostMax = 1000,
                reproductionCount = ReproductionCount,
                reproductionCountMax = ReproductionCountMax,
                effectList = Effects.Select(x => x.GetObjectEffect() as ObjectEffectInteger)
            };
        }

        public MountInformationsForPaddock GetMountInformationsForPaddock()
        {
            return new MountInformationsForPaddock(Model.Id, Name, "");
        }

        #endregion

        public void Save(ORM.Database database)
        {
            WorldServer.Instance.IOTaskPool.ExecuteInContext(() =>
            {
                if (Record.IsNew)
                    database.Insert(Record);
                else
                    database.Update(Record);

                IsDirty = false;
                Record.IsNew = false;
            });
        }
    }
}
