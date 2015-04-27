using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Items.BidHouse;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Accounts;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;

namespace Stump.Server.WorldServer.Game.Items.BidHouse
{
    public class BidHouseItem : PersistantItem<BidHouseItemRecord>
    {
        #region Fields

        public uint Price
        {
            get { return Record.Price; }
            set { Record.Price = value; }
        }

        public bool Sold
        {
            get { return Record.Sold; }
            set { Record.Sold = value; }
        }

        public int UnsoldDelay
        {
            get { return Math.Abs(BidHouseManager.UnsoldDelay - (DateTime.Now - Record.SellDate).Minutes); }
        }

        #endregion

        #region Constructors

        public BidHouseItem(BidHouseItemRecord record)
            : base(record)
        {
            Record = record;
        }

        public BidHouseItem(Character owner, int guid, ItemTemplate template, List<EffectBase> effects, uint stack, uint price, DateTime sellDate)
        {
            Record = new BidHouseItemRecord // create the associated record
            {
                Id = guid,
                OwnerId = owner.Id,
                Template = template,
                Stack = stack,
                Price = price,
                Effects = effects,
                SellDate = sellDate
            };
        }

        #endregion

        #region Functions

        public override ObjectItem GetObjectItem()
        {
            return new ObjectItem(63, (short)Template.Id, 0, false, Effects.Select(x => x.GetObjectEffect()), Guid,
                (int)Stack);
        }

        public ObjectItemToSellInBid GetObjectItemToSellInBid()
        {
            return new ObjectItemToSellInBid((short)Template.Id, 0, false, Effects.Select(x => x.GetObjectEffect()), Guid,
                (int)Stack, (int)Price, (short)UnsoldDelay);
        }

        public bool SellItem(Character buyer)
        {
            if (Price > buyer.Kamas)
            {
                //Vous ne disposez pas d'assez de kamas pour acheter cet objet.
                buyer.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 63);
                return false;
            }

            var character = World.Instance.GetCharacter(x => x.Account.Id == Record.OwnerId);

            if (character == null)
            {
                Sold = true;
            }
            else
            {
                character.Bank.AddKamas((int)Price);

                //Banque : + %1 Kamas (vente de %4 $item%3).
                character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 65, Price, 0, Template.Id, Stack);
            }

            return true;
        }

        #endregion

        public void Save(ORM.Database database)
        {
            WorldServer.Instance.IOTaskPool.AddMessage(() =>
            {
                if (Record.IsNew)
                    database.Insert(Record);
                else
                    database.Update(Record);

                Record.IsDirty = false;
                Record.IsNew = false;
            });
        }
    }
}
