using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Database.Npcs.Replies
{
    [Discriminator("Teleport", typeof(NpcReply), typeof(NpcReplyRecord))]
    public class TeleportReply : NpcReply
    {
        private bool m_mustRefreshPosition;
        private ObjectPosition m_position;

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

        /// <summary>
        /// Parameter 3
        /// </summary>
        public string ItemsParameter
        {
            get
            {
                return Record.GetParameter<string>(3, true);
            }
            set
            {
                Record.SetParameter(3, value);
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

            if (string.IsNullOrEmpty(ItemsParameter))
                return character.Teleport(GetPosition());

            var parameter = ItemsParameter.Split(',');
            var itemsToDelete = new Dictionary<BasePlayerItem, int>();

            foreach (var itemParameter in parameter.Select(x => x.Split('_')))
            {
                int itemId;
                int amount;

                if (!int.TryParse(itemParameter[0], out itemId))
                    return false;

                if (!int.TryParse(itemParameter[1], out amount))
                    return false;

                var template = ItemManager.Instance.TryGetTemplate(itemId);
                if (template == null)
                    return false;

                var item = character.Inventory.TryGetItem(template);

                if (item == null)
                {
                    character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 4);
                    return false;
                }

                if (item.Stack < amount)
                {
                    character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 252);
                    return false;
                }

                itemsToDelete.Add(item, amount);
            }

            foreach (var itemToDelete in itemsToDelete)
            {
                character.Inventory.RemoveItem(itemToDelete.Key, itemToDelete.Value);
            }

            return character.Teleport(GetPosition());
        }
    }
}