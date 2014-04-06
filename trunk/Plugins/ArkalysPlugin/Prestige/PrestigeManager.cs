#region License GNU GPL
// PrestigeManager.cs
// 
// Copyright (C) 2013 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Handlers.Items;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Items.Player;

namespace ArkalysPlugin.Prestige
{
    public class PrestigeManager : Singleton<PrestigeManager>
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        [Variable] public static int ItemForBonus = 20214;

        public static ItemTemplate BonusItem;

        [Variable]
        public static short[] PrestigeTitles =
        {
            200, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210, 211, 212
        };

        private static readonly EffectInteger[][] m_prestigesBonus =
            {
                new EffectInteger[0], 
                new[] {new EffectInteger(EffectsEnum.Effect_AddDamageBonus, 3)},
                new[] {new EffectInteger(EffectsEnum.Effect_AddVitality, 100)},
                new[] {new EffectInteger(EffectsEnum.Effect_AddChance, 25),
                    new EffectInteger(EffectsEnum.Effect_AddIntelligence, 25),
                    new EffectInteger(EffectsEnum.Effect_AddWisdom, 25),
                    new EffectInteger(EffectsEnum.Effect_AddAgility, 25),
                    new EffectInteger(EffectsEnum.Effect_AddStrength, 25)},
                new[] {new EffectInteger(EffectsEnum.Effect_AddVitality, 100)},
                new[] {new EffectInteger(EffectsEnum.Effect_AddChance, 50),
                    new EffectInteger(EffectsEnum.Effect_AddIntelligence, 50),
                    new EffectInteger(EffectsEnum.Effect_AddWisdom, 50),
                    new EffectInteger(EffectsEnum.Effect_AddAgility, 50),
                    new EffectInteger(EffectsEnum.Effect_AddStrength, 50)},
                new[] {new EffectInteger(EffectsEnum.Effect_AddDamageBonus, 9)},
                new[] {new EffectInteger(EffectsEnum.Effect_AddDamageBonusPercent, 25)},
                new[] {new EffectInteger(EffectsEnum.Effect_AddAirElementReduction, 5),
                    new EffectInteger(EffectsEnum.Effect_AddEarthElementReduction, 5),
                    new EffectInteger(EffectsEnum.Effect_AddFireElementReduction, 5),
                    new EffectInteger(EffectsEnum.Effect_AddWaterElementReduction, 5),
                    new EffectInteger(EffectsEnum.Effect_AddNeutralElementReduction, 5)},
                new[] {new EffectInteger(EffectsEnum.Effect_AddDamageBonus, 12)},
                new[] {new EffectInteger(EffectsEnum.Effect_AddDamageBonusPercent, 50)},
                new[] {new EffectInteger(EffectsEnum.Effect_AddAirResistPercent, 5),
                    new EffectInteger(EffectsEnum.Effect_AddEarthResistPercent, 5),
                    new EffectInteger(EffectsEnum.Effect_AddFireResistPercent, 5),
                    new EffectInteger(EffectsEnum.Effect_AddWaterResistPercent, 5),
                    new EffectInteger(EffectsEnum.Effect_AddNeutralResistPercent, 5)},
                new[] {new EffectInteger(EffectsEnum.Effect_AddRange, 1)},
                new[] {new EffectInteger(EffectsEnum.Effect_AddMP, 1)},
                new[] {new EffectInteger(EffectsEnum.Effect_AddAP_111, 1)},

            };

        private bool m_disabled;

        [Initialization(typeof(ItemManager), Silent=true)]
        public void Initialize()
        {
            BonusItem = ItemManager.Instance.TryGetTemplate(ItemForBonus);

            if (BonusItem == null)
            {
                logger.Error("Item {0} not found, prestiges disabled", ItemForBonus);
                m_disabled = true;
            }

            if(!m_disabled)
                World.Instance.CharacterJoined += OnCharacterJoined;
        }

        private void OnCharacterJoined(Character character)
        {
            ApplyPrestigeBonus(character);
        }

        /// <summary>
        /// Returns 0 if the character has no prestige rank
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public int GetPrestigeRank(Character character)
        {
            if (m_disabled)
                return 0;

            if (character.Titles.Count == 0)
                return 1;

            return character.Titles.Max(x => Array.IndexOf(PrestigeTitles, x)) + 1;
        }

        public bool IsPrestigeMax(Character character)
        {
            return GetPrestigeRank(character) == PrestigeTitles.Length;
        }

        public void IncrementPrestige(Character character)
        {
            if (m_disabled)
                return;

            var rank = GetPrestigeRank(character);

            // max prestige already
            if (rank == PrestigeTitles.Length)
                return;

            var title = PrestigeTitles[rank - 1];
            var item = character.Inventory.TryGetItem(BonusItem);

            if (item == null)
                item = character.Inventory.AddItem(BonusItem);
            else
            {
                UnApplyPrestigeBonus(character, item);
            }

            AddEffects(item, m_prestigesBonus[rank].Select(x => (EffectInteger) x.Clone()));
            item.Invalidate();

            ApplyPrestigeBonus(character, item);
            character.AddTitle(title);
            character.Inventory.RefreshItem(item);

            character.LevelDown(character.Level);

            foreach(var equippedItem in character.Inventory)
                character.Inventory.MoveItem(equippedItem, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);

            character.RefreshStats();
            character.RefreshActor();

            character.OpenPopup(
                string.Format("Vous venez de passer au rang prestige {0}. \nVous repassez niveau 1 et vous avez acquis des bonus permanents visible sur l'objet '{1}' de votre inventaire, ", rank+1, BonusItem.Name) +
                "les bonus s'appliquent sans équipper l'objet");
        }

        private static void AddEffects(BasePlayerItem item, IEnumerable<EffectInteger> effects)
        {
            foreach (var effect in effects)
            {
                var existingEffect =
                    item.Effects.OfType<EffectInteger>().FirstOrDefault(x => x.EffectId == effect.EffectId);

                if (existingEffect != null)
                    existingEffect.Value += effect.Value;
                else
                    item.Effects.Add(effect);
            }
        }

        public void UnApplyPrestigeBonus(Character character)
        {
            var item = character.Inventory.TryGetItem(BonusItem);

            UnApplyPrestigeBonus(character, item);
        }

        private static void UnApplyPrestigeBonus(Character character, BasePlayerItem item)
        {
            if (item == null)
                return;

            foreach (var effect in item.Effects)
            {
                var handler = EffectManager.Instance.GetItemEffectHandler(effect, character, item);
                handler.Operation = ItemEffectHandler.HandlerOperation.UNAPPLY;

                handler.Apply();
            }
        }

        public void ApplyPrestigeBonus(Character character)
        {
            var item = character.Inventory.TryGetItem(BonusItem);

            ApplyPrestigeBonus(character, item);

        }

        private static void ApplyPrestigeBonus(Character character, BasePlayerItem item)
        {
            if (item == null)
                return;

            foreach (var handler in item.Effects.Select(effect => EffectManager.Instance.GetItemEffectHandler(effect, character, item)))
            {
                handler.Operation = ItemEffectHandler.HandlerOperation.APPLY;

                handler.Apply();
            }
        }
    }
}