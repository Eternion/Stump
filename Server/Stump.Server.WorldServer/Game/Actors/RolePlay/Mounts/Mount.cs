using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Cache;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.Mounts;
using Stump.Server.WorldServer.Game.Actors.Look;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Handlers.Items;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Items.Player.Custom;
using Stump.Server.WorldServer.Game.Maps.Paddocks;
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
            new[] {60, 1.5},
            new double[] {70, 1}
        };

        [Variable(true)]
        public static int RequiredLevel = 60;


        public Mount(Character character, MountRecord record)
        {
            Record = record;
            Level = ExperienceManager.Instance.GetMountLevel(Experience);
            ExperienceLevelFloor = ExperienceManager.Instance.GetMountLevelExperience(Level);
            ExperienceNextLevelFloor = ExperienceManager.Instance.GetMountNextLevelExperience(Level);
            if (Record.PaddockId != null)
                Paddock = PaddockManager.Instance.GetPaddock(Record.PaddockId.Value);
            m_effects = MountManager.Instance.GetMountEffects(this);

            Owner = character;
        }

        public Mount(MountRecord record)
        {
            Record = record;
            Level = ExperienceManager.Instance.GetMountLevel(Experience);
            ExperienceLevelFloor = ExperienceManager.Instance.GetMountLevelExperience(Level);
            ExperienceNextLevelFloor = ExperienceManager.Instance.GetMountNextLevelExperience(Level);

            if (Record.PaddockId != null)
                Paddock = PaddockManager.Instance.GetPaddock(Record.PaddockId.Value);
            m_effects = MountManager.Instance.GetMountEffects(this);
        }

        public void ApplyMountEffects(bool send = true)
        {
            if (Owner == null)
                return;

            // dummy item
            var item = ItemManager.Instance.CreatePlayerItem(Owner, MountTemplate.DEFAULT_SCROLL_ITEM, 1);
            item.Effects.AddRange(Effects);

            Owner.Inventory.ApplyItemEffects(item, send, ItemEffectHandler.HandlerOperation.APPLY);
        }

        public void UnApplyMountEffects()
        {
            if (Owner == null)
                return;

            // dummy item
            var item = ItemManager.Instance.CreatePlayerItem(Owner, MountTemplate.DEFAULT_SCROLL_ITEM, 1);
            item.Effects.AddRange(Effects);

            Owner.Inventory.ApplyItemEffects(item);
        }

        public void RenameMount(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || Owner == null)
                return;

            Name = name.EscapeString();

            MountHandler.SendMountRenamedMessage(Owner.Client, Id, name);
        }

        public void Sterelize(Character character)
        {
            character.EquippedMount.ReproductionCount = -1;
            MountHandler.SendMountSterelizeMessage(character.Client, character.EquippedMount.Id);
        }

        public void SetGivenExperience(Character character, sbyte xp)
        {
            GivenExperience = xp > 90 ? (sbyte) 90 : (xp < 0 ? (sbyte) 0 : xp);

            MountHandler.SendMountXpRatioMessage(character.Client, GivenExperience);
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
            var behaviors = Record.Behaviors.ToList();
            behaviors.Add((int)behavior);

            Record.Behaviors = behaviors;

            IsDirty = true;
        }

        protected virtual void OnLevelChanged(Character character)
        {
            ExperienceLevelFloor = ExperienceManager.Instance.GetMountLevelExperience(Level);
            ExperienceNextLevelFloor = ExperienceManager.Instance.GetMountNextLevelExperience(Level);

            UnApplyMountEffects();
            m_effects = MountManager.Instance.GetMountEffects(this);
            ApplyMountEffects();

            MountHandler.SendMountSetMessage(character.Client, GetMountClientData());
        }

        public long AdjustGivenExperience(Character giver, long amount)
        {
            var gap = giver.Level - Level;

            for (var i = XP_PER_GAP.Length - 1; i >= 0; i--)
            {
                if (gap > XP_PER_GAP[i][0])
                    return (long) (amount*XP_PER_GAP[i][1]*0.01);
            }

            return (long) (amount*XP_PER_GAP[0][1]*0.01);
        }

        public void Save(ORM.Database database)
        {
            if (IsDirty || Record.IsNew)
            {
                WorldServer.Instance.IOTaskPool.ExecuteInContext(() =>
                {
                    if (Record.IsNew)
                        database.Insert(Record);
                    else if (Record.IsDirty)
                        database.Update(Record);

                    IsDirty = false;
                    Record.IsNew = false;
                });
            }
        }

        public void RefreshMount()
        {
            MountHandler.SendMountSetMessage(Owner.Client, GetMountClientData());
        }

        #region Properties

        public MountRecord Record
        {
            get;
        }

        public bool IsDirty
        {
            get { return Record.IsDirty; }
            set { Record.IsDirty = value; }
        }


        public int Id
        {
            get { return Record.Id; }
            private set { Record.Id = value; }
        }

        public Character Owner
        {
            get { return m_owner; }
            set
            {
                m_owner = value;
                Record.OwnerId = value?.Id;
                Record.OwnerName = value?.Name;
                IsDirty = true;
            }
        }

        private List<EffectInteger> m_effects;
        private Paddock m_paddock;
        private Character m_owner;

        public Paddock Paddock
        {
            get { return m_paddock; }
            set
            {
                m_paddock = value;
                Record.PaddockId = value?.Id;
                IsDirty = true;
            }
        }

        public bool IsInStable
        {
            get { return Record.IsInStable; }
            set
            {
                Record.IsInStable = value;
                IsDirty = true;
            }
        }

        public DateTime? StoredSince
        {
            get { return Record.StoredSince; }
            set
            {
                Record.StoredSince = value;
                IsDirty = true;
            }
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

        public ReadOnlyCollection<EffectInteger> Effects => m_effects.AsReadOnly();

        public ReadOnlyCollection<int> Behaviors => Record.Behaviors.AsReadOnly();

        public MountTemplate Template => Record.Template;

        public int TemplateId
        {
            get { return Record.TemplateId; }
        }

        public ItemTemplate ScrollItem => Template.ScrollItem;

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
            get { return Record.Template.PodsBase + Record.Template.PodsPerLevel*Level; }
        }

        public int FecondationTime
        {
            get { return 0; }
        }

        public bool UseHarnessColors
        {
            get { return Owner?.Record.UseHarnessColor ?? false; }
            set
            {
                if (Owner != null)
                {
                    Owner.Record.UseHarnessColor = value;
                    Owner.UpdateLook();
                    RefreshMount();
                }
            }
        }

        public HarnessItem Harness => Owner?.Inventory.TryGetItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_RIDE_HARNESS) as HarnessItem;

        public ActorLook Look
        {
            get
            {
                var look = Template.EntityLook.Clone();
                var harness = Harness;
                if (harness != null)
                {
                    look.AddSkin((short) harness.HarnessTemplate.SkinId);
                }

                if (harness != null && UseHarnessColors)
                {
                    look.SetColors(harness.HarnessTemplate.Colors);
                }
                else if (Behaviors.Contains((int) MountBehaviorEnum.Caméléone) && Owner != null)
                {
                    if (Owner.DefaultLook.Colors.TryGetValue(3, out var color1) &&
                        Owner.DefaultLook.Colors.TryGetValue(4, out var color2) &&
                        Owner.DefaultLook.Colors.TryGetValue(5, out var color3))
                        look.SetColors(color1, color2, color3);
                }

                return look;
            }
        }

        #endregion
        
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
                model = Template.Id,
                ancestor = new int[0],
                behaviors = Behaviors,
                name = Name,
                ownerId = Record.OwnerId ?? -1,
                experience = Experience,
                experienceForLevel = ExperienceLevelFloor,
                experienceForNextLevel = ExperienceNextLevelFloor,
                level = (sbyte) Level,
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
                effectList = Effects.Select(x => x.GetObjectEffect() as ObjectEffectInteger).
                    Concat(Harness != null ? new [] {new EffectInteger(EffectsEnum.Effect_HarnessGID, (short)Harness.Template.Id).GetObjectEffect() as ObjectEffectInteger} : new ObjectEffectInteger[0]),
                harnessGID = (short)(Harness?.Template.Id ?? 0),
                useHarnessColors = UseHarnessColors,
            };
        }

        public MountInformationsForPaddock GetMountInformationsForPaddock()
            => new MountInformationsForPaddock((sbyte) TemplateId, Name, Record.OwnerName);

        #endregion
    }
}