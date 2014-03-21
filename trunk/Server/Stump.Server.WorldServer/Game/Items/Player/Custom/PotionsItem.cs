﻿using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Items.Player.Custom
{
    [ItemId(ItemIdEnum.NamechangePotion)]
    public class NameChangePotion : BasePlayerItem
    {
        public NameChangePotion(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override uint UseItem(uint amount = 1, Cell targetCell = null, Character target = null)
        {
            if (Owner.Record.Rename || Owner.Record.Recolor || Owner.Record.Relook)
            {
                Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_POPUP, 43);
                return 0;
            }

            Owner.Record.Rename = true;
            Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_POPUP, 41);

            return 1;
        }
    }

    [ItemId(ItemIdEnum.ColourchangePotion)]
    public class ColourChangePotion : BasePlayerItem
    {
        public ColourChangePotion(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override uint UseItem(uint amount = 1, Cell targetCell = null, Character target = null)
        {
            if (Owner.Record.Rename || Owner.Record.Recolor || Owner.Record.Relook)
            {
                Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_POPUP, 43);
                return 0;
            }

            Owner.Record.Recolor = true;
            Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_POPUP, 42);

            return 1;
        }
    }

    [ItemId(ItemIdEnum.LookChangePotion)]
    public class LookChangePotion : BasePlayerItem
    {
        public LookChangePotion(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override uint UseItem(uint amount = 1, Cell targetCell = null, Character target = null)
        {
            if (Owner.Record.Rename || Owner.Record.Recolor || Owner.Record.Relook)
            {
                Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_POPUP, 43);
                return 0;
            }

            Owner.Record.Relook = true;
            Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_POPUP, 58);

            return 1;
        }
    }
}
