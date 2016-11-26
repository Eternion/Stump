using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Maps.Cells;
using System.Collections.Generic;
using Stump.Server.WorldServer.Game.Items.Player;
using System.Linq;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Game.Interactives.Skills
{
    [Discriminator("Teleport", typeof(Skill), typeof(int), typeof(InteractiveCustomSkillRecord), typeof(InteractiveObject))]
    public class SkillTeleport : CustomSkill
    {
        bool m_mustRefreshPosition;
        ObjectPosition m_position;

        public SkillTeleport(int id, InteractiveCustomSkillRecord record, InteractiveObject interactiveObject)
            : base (id, record, interactiveObject)
        {
        }

        public override int StartExecute(Character character)
        {
            if (!Record.AreConditionsFilled(character))
            {
                character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 1);
                return -1;
            }

            if (ItemsParameter != null)
            {
                var parameter = ItemsParameter.Split(',');
                var itemsToDelete = new Dictionary<BasePlayerItem, int>();

                foreach (var itemParameter in parameter.Select(x => x.Split('_')))
                {
                    int itemId;
                    int amount;

                    if (!int.TryParse(itemParameter[0], out itemId))
                        return -1;

                    if (!int.TryParse(itemParameter[1], out amount))
                        return -1;

                    var template = ItemManager.Instance.TryGetTemplate(itemId);
                    if (template == null)
                        return -1;

                    var item = character.Inventory.TryGetItem(template);

                    if (item == null)
                    {
                        //Vous ne possédez pas l'objet nécessaire.
                        character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 4);
                        return -1;
                    }

                    if (item.Stack < amount)
                    {
                        //Vous ne possédez pas l'objet en quantité suffisante.
                        character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 252);
                        return -1;
                    }

                    itemsToDelete.Add(item, amount);
                }

                foreach (var itemToDelete in itemsToDelete)
                {
                    character.Inventory.RemoveItem(itemToDelete.Key, itemToDelete.Value);
                }
            }

            character.Teleport(GetPosition());
            return base.StartExecute(character);
        }

        void RefreshPosition()
        {
            var map = World.Instance.GetMap(MapId);

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

        public int MapId => Record.GetParameter<int>(0);

        public int CellId => Record.GetParameter<int>(1);

        public DirectionsEnum Direction => (DirectionsEnum)Record.GetParameter<int>(2, true);

        public string ItemsParameter => Record.GetParameter<string>(3, true);
    }
}