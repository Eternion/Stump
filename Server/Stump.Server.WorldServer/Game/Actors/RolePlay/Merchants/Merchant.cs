using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Accounts;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Look;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Dialogs.Merchants;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.Merchants
{
    public class Merchant : NamedActor
    {
        public const short BAG_SKIN = 237;

        private readonly WorldMapMerchantRecord m_record;
        private readonly List<MerchantShopDialog> m_openedDialogs = new List<MerchantShopDialog>();
        private bool m_isRecordDirty;


        public Merchant(Character character)
        {
            var look = character.Look.Clone();

            look.AddSubLook(new SubActorLook(0, SubEntityBindingPointCategoryEnum.HOOK_POINT_CATEGORY_MERCHANT_BAG,
                                             new ActorLook
                                                 {
                                                     BonesID = BAG_SKIN
                                                 }));

            m_record = new WorldMapMerchantRecord
                {
                    CharacterId = character.Id,
                    AccountId = character.Account.Id,
                    Name = character.Name,
                    Map = character.Map,
                    Cell = character.Cell.Id,
                    Direction = (int) character.Direction,
                    EntityLook = look,
                    IsActive = true,
                    MerchantSince = DateTime.Now,
                };

            Bag = new MerchantBag(this, character.MerchantBag);
            Position = character.Position.Clone();
        }

        public Merchant(WorldMapMerchantRecord record)
        {
            m_record = record;
            Bag = new MerchantBag(this);

            if (record.Map == null)
                throw new Exception(string.Format("Merchant's map({0}) not found", record.MapId));

            Position = new ObjectPosition(
                record.Map,
                record.Map.Cells[m_record.Cell],
                (DirectionsEnum)m_record.Direction);
        }

        public WorldMapMerchantRecord Record => m_record;

        public ReadOnlyCollection<MerchantShopDialog> OpenDialogs => m_openedDialogs.AsReadOnly();

        public override int Id
        {
            get { return m_record.CharacterId; }
            protected set { m_record.CharacterId = value; }
        }

        public MerchantBag Bag
        {
            get;
            protected set;
        }

        public override ActorLook Look
        {
            get { return m_record.EntityLook; }
            set { m_record.EntityLook = value; }
        }

        public override string Name => m_record.Name;

        public bool IsRecordDirty
        {
            get { return m_isRecordDirty || Bag.IsDirty; }
            set { m_isRecordDirty = value; }
        }

        protected override void OnDisposed()
        {
            m_record.IsActive = false;

            foreach (var dialog in OpenDialogs.ToArray())
            {
                dialog.Close();
            }

            base.OnDisposed();
        }

        public override bool CanBeSee(Maps.WorldObject byObj) => base.CanBeSee(byObj) && !IsBagEmpty();

        public bool IsBagEmpty() => Bag.Count == 0;

        public void LoadRecord()
        {
            Bag.LoadRecord();
        }

        public void Save(ORM.Database database)
        {
            WorldServer.Instance.IOTaskPool.AddMessage(() =>
            {
                if (Bag.IsDirty)
                    Bag.Save(database);

                database.Update(m_record);
            });
        }

        public bool IsMerchantOwner(WorldAccount account) => account.Id == m_record.AccountId;

        public void OnDialogOpened(MerchantShopDialog dialog)
        {
            m_openedDialogs.Add(dialog);
        }

        public void OnDialogClosed(MerchantShopDialog dialog)
        {
            m_openedDialogs.Remove(dialog);
        }

        #region Network

        public override GameContextActorInformations GetGameContextActorInformations(Character character)
        {
            return new GameRolePlayMerchantInformations(Id, Look.GetEntityLook(), GetEntityDispositionInformations(), Name, 0, new HumanOption[0]);
        }

        #endregion

        public override string ToString() => string.Format("{0} ({1})", Name, Id);
    }
}