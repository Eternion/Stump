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

        /// <summary>
        /// Parameter 4
        /// </summary>
        public int KamasParameter
        {
            get
            {
                return Record.GetParameter<int>(4, true);
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

        public override bool CanShow(Npc npc, Character character) => base.CanShow(npc, character) && MapId != character.Map.Id;

        public override bool Execute(Npc npc, Character character)
        {
            if (!base.Execute(npc, character))
                return false;

            if (string.IsNullOrEmpty(ItemsParameter) && KamasParameter == 0)
                return character.Teleport(GetPosition());

            if (character.Kamas < KamasParameter)
            {
                //Vous n'avez pas assez de kamas pour effectuer cette action.
                character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 82);
                return false;
            }

            if (ItemsParameter != null)
            {
                var parameter = ItemsParameter.Split(',');
                var itemsToDelete = new Dictionary<BasePlayerItem, int>();

                foreach (var itemParameter in parameter.Select(x => x.Split('_')))
                {

                    if (!int.TryParse(itemParameter[0], out int itemId))
                        return false;

                    if (!int.TryParse(itemParameter[1], out int amount))
                        return false;

                    var template = ItemManager.Instance.TryGetTemplate(itemId);
                    if (template == null)
                        return false;

                    var item = character.Inventory.TryGetItem(template);

                    if (item == null)
                    {
                        //Vous ne possédez pas l'objet nécessaire.
                        character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 4);
                        return false;
                    }

                    if (item.Stack < amount)
                    {
                        //Vous ne possédez pas l'objet en quantité suffisante.
                        character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 252);
                        return false;
                    }

                    itemsToDelete.Add(item, amount);
                }

                foreach (var itemToDelete in itemsToDelete)
                {
                    character.Inventory.RemoveItem(itemToDelete.Key, itemToDelete.Value);
                }
            }

            character.Inventory.SubKamas(KamasParameter);

            return character.Teleport(GetPosition());
        }
    }
}