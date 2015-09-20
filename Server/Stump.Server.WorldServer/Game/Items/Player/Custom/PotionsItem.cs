using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Dialogs.Guilds;
using Stump.Server.WorldServer.Handlers.Mounts;

namespace Stump.Server.WorldServer.Game.Items.Player.Custom
{
    [ItemId(ItemIdEnum.BONTARIAN_INTERCITY_EXPRESS_POTION)]
    public class BontarianPotion : BasePlayerItem
    {
        [Variable]
        private const int m_destinationMap = 5506048;

        [Variable]
        private const int m_destinationCell = 359;

        public BontarianPotion(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override uint UseItem(int amount = 1, Cell targetCell = null, Character target = null)
        {
            var map = World.Instance.GetMap(m_destinationMap);
            var cell = map.Cells[m_destinationCell];

            Owner.Teleport(map, cell);

            return 1;
        }
    }

    [ItemId(ItemIdEnum.BRAKMARIAN_INTERCITY_EXPRESS_POTION)]
    public class BrakmarianPotion : BasePlayerItem
    {
        [Variable]
        private const int m_destinationMap = 13631488;

        [Variable]
        private const int m_destinationCell = 373;

        public BrakmarianPotion(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override uint UseItem(int amount = 1, Cell targetCell = null, Character target = null)
        {
            var map = World.Instance.GetMap(m_destinationMap);
            var cell = map.Cells[m_destinationCell];

            Owner.Teleport(map, cell);

            return 1;
        }
    }

    [ItemId(ItemIdEnum.NAME_CHANGE_POTION)]
    public class NameChangePotion : BasePlayerItem
    {
        public NameChangePotion(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override uint UseItem(int amount = 1, Cell targetCell = null, Character target = null)
        {
            if ((Owner.Record.MandatoryChanges & (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_NAME)
                == (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_NAME)
            {
                Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_POPUP, 43);
                return 0;
            }

            Owner.Record.MandatoryChanges |= (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_NAME;
            Owner.Record.PossibleChanges |= (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_NAME;

            Owner.SendSystemMessage(41, false);

            return 1;
        }
    }

    [ItemId(ItemIdEnum.COLOUR_CHANGE_POTION)]
    public class ColourChangePotion : BasePlayerItem
    {
        public ColourChangePotion(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override uint UseItem(int amount = 1, Cell targetCell = null, Character target = null)
        {
            if ((Owner.Record.MandatoryChanges & (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_COLORS)
                == (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_COLORS)
            {
                Owner.SendSystemMessage(43, false);
                return 0;
            }

            Owner.Record.MandatoryChanges |= (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_COLORS;
            Owner.Record.PossibleChanges |= (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_COLORS;

            Owner.SendSystemMessage(42, false);

            return 1;
        }
    }

    [ItemId(ItemIdEnum.FACE_CHANGE_POTION)]
    public class LookChangePotion : BasePlayerItem
    {
        public LookChangePotion(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override uint UseItem(int amount = 1, Cell targetCell = null, Character target = null)
        {
            if ((Owner.Record.MandatoryChanges & (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_COSMETIC)
                == (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_COSMETIC)
            {
                Owner.SendSystemMessage(43, false);
                return 0;
            }

            Owner.Record.MandatoryChanges |= (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_COSMETIC;
            Owner.Record.PossibleChanges |= (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_COSMETIC;

            Owner.SendSystemMessage(58, false);

            return 1;
        }
    }

    [ItemId(ItemIdEnum.SEX_CHANGE_POTION)]
    public class SexChangePotion : BasePlayerItem
    {
        public SexChangePotion(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override uint UseItem(int amount = 1, Cell targetCell = null, Character target = null)
        {
            if ((Owner.Record.MandatoryChanges & (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_GENDER)
                == (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_GENDER)
            {
                Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_POPUP, 43);
                Owner.SendSystemMessage(43, false);
                return 0;
            }

            Owner.Record.MandatoryChanges |= (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_GENDER;
            Owner.Record.PossibleChanges |= (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_GENDER;
            Owner.Record.PossibleChanges |= (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_COSMETIC;

            Owner.SendSystemMessage(44, false);

            return 1;
        }
    }

    [ItemId(ItemIdEnum.CLASS_CHANGE_POTION)]
    public class ClassChangePotion : BasePlayerItem
    {
        public ClassChangePotion(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override uint UseItem(int amount = 1, Cell targetCell = null, Character target = null)
        {
            if ((Owner.Record.MandatoryChanges & (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_BREED)
                == (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_BREED)
            {
                Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_POPUP, 43);
                Owner.SendSystemMessage(43, false);
                return 0;
            }

            Owner.Record.MandatoryChanges |= (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_BREED;
            Owner.Record.PossibleChanges |= (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_BREED;
            Owner.Record.PossibleChanges |= (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_GENDER;
            Owner.Record.PossibleChanges |= (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_COSMETIC;
            Owner.Record.PossibleChanges |= (sbyte)CharacterRemodelingEnum.CHARACTER_REMODELING_COLORS;

            Owner.SendSystemMessage(63, false);

            return 1;
        }
    }

    [ItemId(ItemIdEnum.GUILD_RENAMING_POTION)]
    public class GuildNameChangePotion : BasePlayerItem
    {
        public GuildNameChangePotion(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override uint UseItem(int amount = 1, Cell targetCell = null, Character target = null)
        {
            if (Owner.GuildMember == null)
                return 0;

            if (!Owner.GuildMember.IsBoss)
                return 0;

            var panel = new GuildModificationPanel(Owner) { ChangeName = true, ChangeEmblem = false };
            panel.Open();

            return 0;
        }
    }

    [ItemId(ItemIdEnum.GUILD_EMBLEM_CHANGE_POTION)]
    public class GuildEmblemChangePotion : BasePlayerItem
    {
        public GuildEmblemChangePotion(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override uint UseItem(int amount = 1, Cell targetCell = null, Character target = null)
        {
            if (Owner.GuildMember == null)
                return 0;

            if (!Owner.GuildMember.IsBoss)
                return 0;

            var panel = new GuildModificationPanel(Owner) { ChangeName = false, ChangeEmblem = true };
            panel.Open();

            return 0;
        }
    }

    //Item doesn't exist anymore
    [ItemId(20838)]
    public class ChameleonBehaviorPotion : BasePlayerItem
    {
        public ChameleonBehaviorPotion(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override uint UseItem(int amount = 1, Cell targetCell = null, Character target = null)
        {
            if (!Owner.HasEquipedMount())
                return 0;

            if (Owner.Mount.Behaviors.Contains(MountBehaviorEnum.Caméléone))
                return 0;

            Owner.Mount.AddBehavior(MountBehaviorEnum.Caméléone);

            MountHandler.SendMountSetMessage(Owner.Client, Owner.Mount.GetMountClientData());

            return 1;
        }
    }
}
