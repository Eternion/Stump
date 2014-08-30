using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Database.Npcs.Replies
{
    [Discriminator("Teleport", typeof(NpcReply), typeof(NpcReplyRecord))]
    public class TeleportReply : NpcReply
    {
        private bool m_mustRefreshPosition;
        private ObjectPosition m_position;
        private ItemTemplate m_itemTemplate;

        public TeleportReply()
        {
            Record.Type = "Teleport";
        }

        public TeleportReply(NpcReplyRecord record)
            : base(record)
        {
        }

        /// <summary>
        /// Parameter 0
        /// </summary>
        public int MapId
        {
            get
            {
                return Record.GetParameter<int>(0);
            }
            set
            {
                Record.SetParameter(0, value);
                m_mustRefreshPosition = true;
            }
        }

        /// <summary>
        /// Parameter 1
        /// </summary>
        public int CellId
        {
            get
            {
                return Record.GetParameter<int>(1);
            }
            set
            {
                Record.SetParameter(1, value);
                m_mustRefreshPosition = true;
            }
        }

        /// <summary>
        /// Parameter 2
        /// </summary>
        public DirectionsEnum Direction
        {
            get
            {
                return (DirectionsEnum)Record.GetParameter<int>(2);
            }
            set
            {
                Record.SetParameter(2, (int)value);
                m_mustRefreshPosition = true;
            }
        }

        public int ItemId
        {
            get { return Record.GetParameter<int>(3); }
            set { Record.SetParameter(3, value); }
        }

        public ItemTemplate Item
        {
            get { return m_itemTemplate ?? (m_itemTemplate = ItemManager.Instance.TryGetTemplate(ItemId)); }
            set
            {
                m_itemTemplate = value;
                ItemId = value.Id;
            }
        }

        /// <summary>
        /// Parameter 1
        /// </summary>
        public uint Amount
        {
            get
            {
                return Record.GetParameter<uint>(4);
            }
            set
            {
                Record.SetParameter(4, value);
            }
        }

        private void RefreshPosition()
        {
            var map = Game.World.Instance.GetMap(MapId);

            if (map == null)
                throw new Exception(string.Format("Cannot load SkillTeleport id={0}, map {1} isn't found", Id, MapId));

            var cell = map.Cells[CellId];

            m_position = new ObjectPosition(map, cell, Direction);
        }

        public ObjectPosition GetPosition()
        {
            if (m_position == null || m_mustRefreshPosition)
                RefreshPosition();

            m_mustRefreshPosition = false;

            return m_position;
        }

        public override bool Execute(Npc npc, Character character)
        {
            if (!base.Execute(npc, character))
                return false;

            if (Item == null)
                return character.Teleport(GetPosition());

            var item = character.Inventory.TryGetItem(Item);

            if (item == null)
                return false;

            if (item.Stack < Amount)
                return false;

            character.Inventory.RemoveItem(item, (int)Amount);
            character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 22, Amount,
                item.Template.Id);

            return character.Teleport(GetPosition());
        }
    }
}